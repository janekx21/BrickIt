using System.Collections.Generic;
using UnityEngine;

namespace GamePlay {

    // todo
    // make a scriptable object named "Theme" and put this into it
    public class ColorConversion {
        public static Color Convert(ColorType colorType) {
            switch (colorType) {
                case ColorType.red:
                    return Color.red;
                case ColorType.blue:
                    return new Color32(0x01, 0xA3, 0xFF, 0xFF);
                case ColorType.yellow:
                    return Color.yellow;
                case ColorType.green:
                    return Color.green;
                case ColorType.orange:
                    return new Color32(0xFF, 0x80, 0x01, 0xFF);
                case ColorType.cyan:
                    return Color.cyan;
                case ColorType.magenta:
                    return Color.magenta;
                default:
                    return Color.white;
            }
        }


        // todo move to better place
        public static List<ColorType> allColors = new() {
            ColorType.@default, ColorType.red, ColorType.blue, ColorType.green, ColorType.yellow, ColorType.orange,
            ColorType.cyan, ColorType.magenta
        };
    }
}