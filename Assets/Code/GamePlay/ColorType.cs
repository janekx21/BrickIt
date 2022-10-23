using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay {
    public enum ColorType {
        Default, // white in most cases
        Red,
        Blue,
        Green,
        Yellow,
        Orange,
        Cyan,
        Magenta
    }

    // todo
    // make a scriptable object named "Theme" and put this into it
    public class ColorConversion {
        public static Color Convert(ColorType colorType) {
            switch (colorType) {
                case ColorType.Red:
                    return Color.red;
                case ColorType.Blue:
                    return new Color32(0x01, 0xA3, 0xFF, 0xFF);
                case ColorType.Yellow:
                    return Color.yellow;
                case ColorType.Green:
                    return Color.green;
                case ColorType.Orange:
                    return new Color32(0xFF, 0x80, 0x01, 0xFF);
                case ColorType.Cyan:
                    return Color.cyan;
                case ColorType.Magenta:
                    return Color.magenta;
                default:
                    return Color.white;
            }
        }


        // todo move to better place
        public static List<ColorType> allColors = new() {
            ColorType.Default, ColorType.Red, ColorType.Blue, ColorType.Green, ColorType.Yellow, ColorType.Orange,
            ColorType.Cyan, ColorType.Magenta
        };
    }
}