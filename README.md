# DoNotIncludeDefaultResourceForSize
![Supporting Status: End of Life](https://img.shields.io/badge/support-end_of_life-red?style=for-the-badge) ![Works with Neos](https://img.shields.io/badge/works_with-neos-yellow?style=for-the-badge)
![Requires NML 1.12.6 or later](https://img.shields.io/badge/neos_mod_loader-%3E%3D%201.12.6-409c41?style=for-the-badge)

**NOTE**: This MOD do not receive any further support including security update.

18MB problem is no longer an issue.

## What is this?
This MOD excludes built-in font from Inventory size computation.
This MOD requires NeosModLoader (version: `1.12.5`).

## But... why?
Those fonts are not counted as your storage usage, so I thought these assets can be excluded safely from measurement of bytes.
```cs
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
```

Those fonts are about 18MB, so I call this "18MB problem". Without this MOD, a empty SpaceWorld *consumes* about 18MB.
But Noto Sans CJK JP Medium has major impact: it is roughly 16.5MB.

