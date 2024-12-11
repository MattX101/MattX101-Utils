using UnityEngine;
using Utils.Colors.Model;

namespace Utils.Colors
{
    // Credits
    // https://www.rapidtables.com/convert/color/rgb-to-hsl.html
    // https://www.rapidtables.com/convert/color/hsl-to-rgb.html
    // https://www.rapidtables.com/convert/color/rgb-to-hsv.html
    // https://www.rapidtables.com/convert/color/hsv-to-rgb.html

    public static class ColorConversion
    {
        // HEX
        public static HEX RGBToHex(Color color)
        {
            int r = (int)(color.r * 255);
            int g = (int)(color.g * 255);
            int b = (int)(color.b * 255);

            int rA = Mathf.FloorToInt(r / 16);
            int gA = Mathf.FloorToInt(g / 16);
            int bA = Mathf.FloorToInt(b / 16);

            int rB = r - (16 * rA);
            int gB = g - (16 * gA);
            int bB = b - (16 * bA);

            string hex = "";
            hex += GetHexValue(rA);
            hex += GetHexValue(rB);
            hex += GetHexValue(gA);
            hex += GetHexValue(gB);
            hex += GetHexValue(bA);
            hex += GetHexValue(bB);

            return new HEX(hex);
        }

        public static Color HEXToRGB(HEX hex)
        {
            int rA = GetHexValue(hex.Hex[0]) * 16;
            int rB = GetHexValue(hex.Hex[1]);
            int gA = GetHexValue(hex.Hex[2]) * 16;
            int gB = GetHexValue(hex.Hex[3]);
            int bA = GetHexValue(hex.Hex[4]) * 16;
            int bB = GetHexValue(hex.Hex[5]);

            float r = (rA + rB) / 255.0f;
            float g = (gA + gB) / 255.0f;
            float b = (bA + bB) / 255.0f;

            return new Color(r, g, b, 1);
        }

        private static int GetHexValue(char c)
        {
            return c switch
            {
                '0' => 0,
                '1' => 1,
                '2' => 2,
                '3' => 3,
                '4' => 4,
                '5' => 5,
                '6' => 6,
                '7' => 7,
                '8' => 8,
                '9' => 9,
                'A' => 10,
                'B' => 11,
                'C' => 12,
                'D' => 13,
                'E' => 14,
                'F' => 15,
                _   => 0,
            };
        }

        private static char GetHexValue(int index)
        {
            return index switch
            {
                0  => '0',
                1  => '1',
                2  => '2',
                3  => '3',
                4  => '4',
                5  => '5',
                6  => '6',
                7  => '7',
                8  => '8',
                9  => '9',
                10 => 'A',
                11 => 'B',
                12 => 'C',
                13 => 'D',
                14 => 'E',
                15 => 'F',
                _  => '-'
            };
        }

        private static float Min(Color color) => Mathf.Min(Mathf.Min(color.r, color.g), color.b);
        private static float Max(Color color) => Mathf.Max(Mathf.Max(color.r, color.g), color.b);

        private static float Hue(Color color, float min, float max, float diff)
        {
            float h = 0.0f;
            if (diff > 0.0f)
            {
                if (max == color.r)
                {
                    h = 60.0f * ((color.g - color.b) / diff);
                    h = h < 0 ? h + 360 : h;
                }
                else if (max == color.g)
                {
                    h = 60.0f * (((color.b - color.r) / diff) + 2);
                }
                else if (max == color.b)
                {
                    h = 60.0f * (((color.r - color.g) / diff) + 4);
                }
            }

            return h;
        }

        private static Color ReturnColor(float hue, float C, float X, Color m)
        {
            return hue switch
            {
                >= 300 => new Color(C, 0, X) + m,
                >= 240 => new Color(X, 0, C) + m,
                >= 180 => new Color(0, X, C) + m,
                >= 120 => new Color(0, C, X) + m,
                >= 60  => new Color(X, C, 0) + m,
                _      => new Color(C, X, 0) + m
            };
        }

        // HSL
        public static HSL RGBToHSL(Color color)
        {
            float max = Max(color);
            float min = Min(color);
            float diff = max - min;

            float l = (max + min) / 2.0f;

            return new HSL(
                Hue(color, min, max, diff),
                diff == 0.0f ? 0.0f : diff / (1.0f - Mathf.Abs(2.0f * l - 1.0f)),
                l);
        }

        public static Color HSLToRGB(HSL hsl)
        {
            float C = (1.0f - Mathf.Abs(2 * hsl.Lightness - 1.0f)) * hsl.Saturation;
            float X = C * (1.0f - Mathf.Abs((hsl.Hue / 60.0f) % 2.0f - 1.0f));

            float n = hsl.Lightness - C / 2.0f;

            return ReturnColor(hsl.Hue, C, X, new Color(n, n, n, 0));
        }

        // HSV
        public static HSV RGBToHSV(Color color)
        {
            float max = Max(color);
            float min = Min(color);
            float diff = max - min;

            return new HSV(
                Hue(color, min, max, diff),
                max == 0.0f ? 0.0f : diff / max, 
                max);
        }

        public static Color HSVToRGB(HSV hsv)
        {
            float C = hsv.Value * hsv.Saturation;
            float X = C * (1.0f - Mathf.Abs((hsv.Hue / 60.0f) % 2.0f - 1.0f));

            float n = hsv.Value - C;

            return ReturnColor(hsv.Hue, C, X, new Color(n, n, n, 0));
        }
    }
}
