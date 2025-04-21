using UnityEngine;

namespace Utils.Colors.Blend
{
    internal static class Formulas
    {
        /// Utils
        private static bool LessThan(float v) => v <= 0.5f;
        private static float Sqrt(float v) => Mathf.Sqrt(v);
        private static float Invert(float v) => 1.0f - v;
        private static float TimesTwo(float v) => v * 2;
        private static float Abs(float v) => Mathf.Abs(v);
        private static float Min(float a, float b) => Mathf.Min(a, b);
        private static float Max(float a, float b) => Mathf.Max(a, b);
        private static float Power(float v, float p = 2.0f) => Mathf.Pow(v, p);

        /// Arithmetic
        internal static float Add(float a, float b) => a + b;
        internal static float Subtract(float a, float b) => a - b;
        internal static float Multiply(float a, float b) => a * b;
        internal static float Divide(float a, float b) => a / b;
        internal static float Average(float a, float b) => Divide(Add(a, b), 2.0f);

        /// Darken
        internal static float Darken(float a, float b) => a < b ? a : b;
        internal static float ColorBurn(float a, float b) => Invert(Divide(Invert(a), b));
        internal static float LinearBurn(float a, float b) => Subtract(Add(a, b), 1);
        internal static float GammaDark(float a, float b) => Power(a, Divide(1, b));

        /// Lighten
        internal static float Lighten(float a, float b) => a > b ? a : b;
        internal static float Shine(float a, float b) => Add(Power(b), a);
        internal static float Screen(float a, float b) => Invert(Multiply(Invert(a), Invert(b)));
        internal static float ColorDodge(float a, float b) => Divide(a, Invert(b));
        internal static float Overlay(float a, float b)
        {
            return
                LessThan(a) ?
                TimesTwo(Multiply(a, b)) :
                Invert(TimesTwo(Multiply(Invert(a), Invert(b))));
        }
        internal static float SoftLight(float a, float b)
        {
            return
                LessThan(a) ?
                Multiply(a, Add(b, 0.5f)) :
                Invert(Invert(a) * Invert(Subtract(b, 0.5f)));
        }
        internal static float HardLight(float a, float b)
        {
            return
                LessThan(b) ?
                Multiply(b, TimesTwo(a)) :
                Invert(Multiply(Invert(b), Invert(TimesTwo(Subtract(a, 0.5f)))));
        }
        // Vivid Light
        internal static float LinearLight(float a, float b)
        {
            return
                LessThan(a) ?
                Subtract(Add(a, TimesTwo(b)), 1.0f) :
                Add(a, TimesTwo(Subtract(b, 0.5f)));
        }
        // Pin Light
        internal static float HardMix(float a, float b) => Add(a, b) < 1 ? 0 : 1;
        internal static float Tint(float a, float b) => Invert(Subtract(b, a));
        internal static float GammaLight(float a, float b) => Power(a, Invert(b));
        internal static float GammaIllumination(float a, float b) => Invert(GammaDark(a, b));

        /// Other
        internal static float Exclusion(float a, float b) => Subtract(Add(b, a), TimesTwo(Multiply(a, b)));
        internal static float Difference(float a, float b) => Abs(b - a);
        internal static float Negation(float a, float b)
        {
            float v = Add(a, b);
            return v > 1 ? Add(1.0f, Invert(v)) : v;
        }
    }
}
