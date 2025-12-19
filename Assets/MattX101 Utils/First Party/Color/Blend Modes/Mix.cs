using Utils.Colors.Model;
using UnityEngine;

namespace Utils.Colors.Blend
{
    public static class Mix
    {
        public static ComputeShader Shader
        {
            get;
            set;
        }

        /// Arithmetic
        public static Color Add(Color a, Color b) => new (Formulas.Add(a.r, b.r), Formulas.Add(a.g, b.g), Formulas.Add(a.b, b.b), 1);
        public static Color Subtract(Color a, Color b) => new (Formulas.Subtract(a.r, b.r), Formulas.Subtract(a.g, b.g), Formulas.Subtract(a.b, b.b), 1);
        public static Color Multiply(Color a, Color b) => new (Formulas.Multiply(a.r, b.r), Formulas.Multiply(a.g, b.g), Formulas.Multiply(a.b, b.b), 1);
        public static Color Divide(Color a, Color b) => new (Formulas.Divide(a.r, b.r), Formulas.Divide(a.g, b.g), Formulas.Divide(a.b, b.b), 1);
        public static Color Average(Color a, Color b) => new (Formulas.Average(a.r, b.r), Formulas.Average(a.g, b.g), Formulas.Average(a.b, b.b), 1);

        /// Darken
        public static Color Darken(Color a, Color b) => new (Formulas.Darken(a.r, b.r), Formulas.Darken(a.g, b.g), Formulas.Darken(a.b, b.b), 1);
        public static Color ColorBurn(Color a, Color b) => new (Formulas.ColorBurn(a.r, b.r), Formulas.ColorBurn(a.g, b.g), Formulas.ColorBurn(a.b, b.b), 1);
        public static Color LinearBurn(Color a, Color b) => new (Formulas.LinearBurn(a.r, b.r), Formulas.LinearBurn(a.g, b.g), Formulas.LinearBurn(a.b, b.b), 1);
        public static Color GammaDark(Color a, Color b) => new (Formulas.GammaDark(a.r, b.r), Formulas.GammaDark(a.g, b.g), Formulas.GammaDark(a.b, b.b), 1);

        /// Lighten
        public static Color Lighten(Color a, Color b) => new (Formulas.Lighten(a.r, b.r), Formulas.Lighten(a.g, b.g), Formulas.Lighten(a.b, b.b), 1);
        public static Color Shine(Color a, Color b) => new (Formulas.Shine(a.r, b.r), Formulas.Shine(a.g, b.g), Formulas.Shine(a.b, b.b), 1);
        public static Color Screen(Color a, Color b) => new (Formulas.Screen(a.r, b.r), Formulas.Screen(a.g, b.g), Formulas.Screen(a.b, b.b), 1);
        public static Color ColorDodge(Color a, Color b) => new (Formulas.ColorDodge(a.r, b.r), Formulas.ColorDodge(a.g, b.g), Formulas.ColorDodge(a.b, b.b), 1);
        public static Color Overlay(Color a, Color b) => new (Formulas.Overlay(a.r, b.r), Formulas.Overlay(a.g, b.g), Formulas.Overlay(a.b, b.b), 1);
        public static Color SoftLight(Color a, Color b) => new (Formulas.SoftLight(a.r, b.r), Formulas.SoftLight(a.g, b.g), Formulas.SoftLight(a.b, b.b), 1);
        public static Color HardLight(Color a, Color b) => new (Formulas.HardLight(a.r, b.r), Formulas.HardLight(a.g, b.g), Formulas.HardLight(a.b, b.b), 1);
        public static Color LinearLight(Color a, Color b) => new (Formulas.LinearLight(a.r, b.r), Formulas.LinearLight(a.g, b.g), Formulas.LinearLight(a.b, b.b), 1);
        public static Color HardMix(Color a, Color b) => new (Formulas.HardMix(a.r, b.r), Formulas.HardMix(a.g, b.g), Formulas.HardMix(a.b, b.b), 1);
        public static Color Tint(Color a, Color b) => new (Formulas.Tint(a.r, b.r), Formulas.Tint(a.g, b.g), Formulas.Tint(a.b, b.b), 1);
        public static Color GammaLight(Color a, Color b) => new (Formulas.GammaLight(a.r, b.r), Formulas.GammaLight(a.g, b.g), Formulas.GammaLight(a.b, b.b), 1);
        public static Color GammaIllumination(Color a, Color b) => new (Formulas.GammaIllumination(a.r, b.r), Formulas.GammaIllumination(a.g, b.g), Formulas.GammaIllumination(a.b, b.b), 1);

        /// Other
        public static Color Exclusion(Color a, Color b) => new (Formulas.Exclusion(a.r, b.r), Formulas.Exclusion(a.g, b.g), Formulas.Exclusion(a.b, b.b), 1);
        public static Color Difference(Color a, Color b) => new (Formulas.Difference(a.r, b.r), Formulas.Difference(a.g, b.g), Formulas.Difference(a.b, b.b), 1);
        public static Color Negation(Color a, Color b) => new (Formulas.Negation(a.r, b.r), Formulas.Negation(a.g, b.g), Formulas.Negation(a.b, b.b), 1);

        /// Shift
        public static Color Hue(Color a, Color b)
        {
            HSL hslA = ColorConversion.RGBToHSL(a);
            return ColorConversion.HSLToRGB(new HSL(ColorConversion.RGBToHSL(b).Hue, hslA.Saturation, hslA.Lightness));
        }

        public static Color Saturation(Color a, Color b)
        {
            HSL hslA = ColorConversion.RGBToHSL(a);
            return ColorConversion.HSLToRGB(new HSL(hslA.Hue, ColorConversion.RGBToHSL(b).Saturation, hslA.Lightness));
        }

        public static Color Color(Color a, Color b)
        {
            HSL hslB = ColorConversion.RGBToHSL(b);
            return ColorConversion.HSLToRGB(new HSL(hslB.Hue, hslB.Saturation, ColorConversion.RGBToHSL(a).Lightness));
        }

        public static Color Luminosity(Color a, Color b)
        {
            HSL hslA = ColorConversion.RGBToHSL(a);
            return ColorConversion.HSLToRGB(new HSL(hslA.Hue, hslA.Saturation, ColorConversion.RGBToHSL(b).Lightness));
        }

        /// GPU Compute
        
        public static void Blend(ref ComputeBuffer resultBuffer, ComputeBuffer baseBuffer, ComputeBuffer blendBuffer, Blends blendMode)
        {
            int kernel = GetKernel(blendMode);

            Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("result"), resultBuffer);
            Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("base"), baseBuffer);
            Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("blend"), blendBuffer);

            Shader.Dispatch(kernel, Mathf.CeilToInt(resultBuffer.count / 1024.0f), 1, 1);
        }

        private static int GetKernel(Blends mode)
        {
            return mode switch
            {
                /// Arithmetic
                Blends.Add => Shader.FindKernel("Add"),
                Blends.Subtract => Shader.FindKernel("Subtract"),
                Blends.Multiply => Shader.FindKernel("Multiply"),
                Blends.Divide => Shader.FindKernel("Divide"),
                Blends.Average => Shader.FindKernel("Average"),

                /// Darken
                Blends.Darken => Shader.FindKernel("Darken"),
                Blends.ColorBurn => Shader.FindKernel("ColorBurn"),
                Blends.LinearBurn => Shader.FindKernel("LinearBurn"),
                Blends.GammaDark => Shader.FindKernel("GammaDark"),

                /// Lighten
                Blends.Lighten => Shader.FindKernel("Lighten"),
                Blends.Shine => Shader.FindKernel("Shine"),
                Blends.ColorDodge => Shader.FindKernel("ColorDodge"),
                Blends.Screen => Shader.FindKernel("Screen"),
                Blends.Overlay => Shader.FindKernel("Overlay"),
                Blends.SoftLight => Shader.FindKernel("SoftLight"),
                Blends.HardLight => Shader.FindKernel("HardLight"),
                Blends.LinearLight => Shader.FindKernel("LinearLight"),
                Blends.HardMix => Shader.FindKernel("HardMix"),
                Blends.Tint => Shader.FindKernel("Tint"),
                Blends.GammaLight => Shader.FindKernel("GammaLight"),
                Blends.GammaIllumination => Shader.FindKernel("GammaIllumination"),

                /// Other
                Blends.Exclusion => Shader.FindKernel("Exclusion"),
                Blends.Difference => Shader.FindKernel("Difference"),
                Blends.Negation => Shader.FindKernel("Negation"),

                /// Default
                _ => Shader.FindKernel("Add")
            };
        }
    }
}
