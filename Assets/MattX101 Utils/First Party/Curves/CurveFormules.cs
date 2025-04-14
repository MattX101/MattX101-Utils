using UnityEngine;

namespace Utils.Curves
{
    internal static class CurveFormules
    {
        private const float PI = 3.1415f;
        private const float Radian = 0.841471f;

        // Easing Functions
        internal static float EaseIn(float v, float p)
        {
            return Mathf.Pow(v, p);
        }

        internal static float EaseInCirc(float v, float p)
        {
            return 1.0f - Mathf.Sqrt(1.0f - Mathf.Pow(v, p));
        }

        internal static float EaseInOut(float v, float p)
        {
            return
                v < 0.5f ?
                Mathf.Pow(2, p - 1) * Mathf.Pow(v, p) :
                1.0f - (Mathf.Pow(-2 * v + 2, p) / 2.0f);
        }

        internal static float EaseInOutSine(float v, float p)
        {
            return Mathf.Pow(-(Mathf.Cos(PI * v) - 1) / 2.0f, p);
        }

        // Sin Functions
        internal static float Sine(float v, float p)
        {
            return Mathf.Abs(Mathf.Pow(Mathf.Sin(PI * v), p));
        }

        internal static float SineSqrt(float v, float p)
        {
            return Mathf.Sqrt(Mathf.Abs(Mathf.Pow(Mathf.Sin(PI * v), p)));
        }

        internal static float RepeatedSine(float v, float p)
        {
            return
                v < 0.5f ?
                Mathf.Pow(Mathf.Sin(PI * v), p) / 2.0f :
                1.0f + (-Mathf.Pow(Mathf.Sin(PI * v), p) / 2.0f);
        }

        internal static float SineFrequency(float v, float p)
        {
            return Mathf.Sin(PI * Mathf.Pow(v, p));
        }

        internal static float RadianArcSine(float v, float p)
        {
            return Mathf.Pow(Mathf.Asin(v * Radian), p);
        }

        internal static float RadianArcSineSqrt(float v, float p)
        {
            return Mathf.Sqrt(Mathf.Pow(Mathf.Asin(v * Radian), p));
        }

        internal static float HalfDownUpSine(float v, float p)
        {
            return
                v < 0.5f ?
                0.5f + (-Mathf.Pow(Mathf.Sin(PI * v), p)) / 2.0f :
                1.0f + -Mathf.Pow(Mathf.Sin(PI * v), p);
        }
    }
}
