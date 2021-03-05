using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay {
    public enum ColorType {
        Default,
        Red,
        Blue,
        Green,
        Yellow,
        Orange,
        Cyan,
        Magenta
    }

    public class ColorConversion {
        public static Color GetColorFromType(ColorType colorType) {
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
    }
}