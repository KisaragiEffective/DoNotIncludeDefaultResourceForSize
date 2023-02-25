using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using BaseX;
using FrooxEngine;
using HarmonyLib;
using JetBrains.Annotations;
using NeosModLoader;
using Record = FrooxEngine.Record;

namespace DoNotIncludeDefaultResourceForSize
{
    [UsedImplicitly]
    public class Mod : NeosMod
    {
        public override string Name => "DoNotIncludeDefaultResourceForSize";
        public override string Author => "kisaragi marine";
        public override string Version => "0.1.4";

        public override void OnEngineInit()
        {
            var harmony = new Harmony("com.github.kisaragieffective.neos.donotincludedefaultresourceforsize");
            harmony.PatchAll();
            Debug("Injected");
        }
    }

    [HarmonyPatch(typeof(InventoryBrowser), "GetSelectedText")]
    internal class Patch
    {
        private static readonly FieldInfo ItemField = typeof(InventoryItemUI).GetField("Item",
            BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly IImmutableSet<string> BuiltinFontAssetHash = ImmutableHashSet.Create(
            // Noto Sans Math Regular
            "23e7ad7cb0a5a4cf75e07c9e0848b1eb06bba15e8fa9b8cb0579fc823c532927",
            // Noto EmojiRegularMonotype Imaging
            "415dc6290378574135b64c808dc640c1df7531973290c4970c51fdeb849cb0c5",
            // Noto Sans Symbols2Regular2
            "4cac521169034ddd416c6deffe2eb16234863761837df677a910697ec5babd25",
            // Noto Sans CJK JP Medium
            "bcda0bcc22bab28ea4fedae800bfbf9ec76d71cc3b9f851779a35b7e438a839d",
            // Noto Sans Medium
            "c801b8d2522fb554678f17f4597158b1af3f9be3abd6ce35d5a3112a81e2bf39"
        );
        
        [HarmonyPrefix]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [method:UsedImplicitly]
        internal static bool patch(InventoryBrowser __instance, ref string __result)
        {
            var selected = __instance.SelectedInventoryItem;
            if (selected == null)
            {
                return true;
            }

            var i = (Record) ItemField.GetValue(selected);

            if (i == null)
            {
                return true;
            }
            
            NeosMod.Debug("Counting.");
            NeosMod.Debug($"Started for {i.id} ({i.Name})");
            var m = i.NeosDBManifest;
            if (m == null)
            {
                // The selected item is not owned by current user
                return true;
            }

            m.ForEach(x =>
            {
                NeosMod.Debug($"neosdb:///{x.Hash} | {x.Bytes} byte");
            });

            var recomputedSize = m.Where(a => !BuiltinFontAssetHash.Contains(a.Hash)).Sum(a => a.Bytes);
            NeosMod.Debug($"Size (excluded built-in fonts): {recomputedSize}");
            __result = selected.ItemName + " (" + UnitFormatting.FormatBytes(recomputedSize) + ")";

            NeosMod.Debug("Ended.");
            return false;
        }
    }
}