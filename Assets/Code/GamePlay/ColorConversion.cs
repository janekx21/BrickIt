using System.Collections.Generic;
using Model;
using UnityEngine;

namespace GamePlay {

    // todo
    // make a scriptable object named "Theme" and put this into it
    public static class ColorConversion {
        public static Color Convert(ColorType colorType) =>
            colorType switch {
                ColorType.red => Color.red,
                ColorType.blue => new Color32(0x01, 0xA3, 0xFF, 0xFF),
                ColorType.yellow => Color.yellow,
                ColorType.green => Color.green,
                ColorType.orange => new Color32(0xFF, 0x80, 0x01, 0xFF),
                ColorType.cyan => Color.cyan,
                ColorType.magenta => Color.magenta,
                _ => Color.white
            };


        // todo move to better place
        public static readonly List<ColorType> allColors = new() {
            ColorType.@default, ColorType.red, ColorType.blue, ColorType.green, ColorType.yellow, ColorType.orange,
            ColorType.cyan, ColorType.magenta
        };
    }
}
