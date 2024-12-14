using UnityEngine;

namespace Utils.Curves
{
    public static class Curves
    {
        private static float PI = 3.1415f;
        private static float Radian = 0.841471f;

        // Easing Functions
        public static float EaseIn(float v, float p)
        {
            return Mathf.Pow(v, p);
        }

        public static float EaseInCirc(float v, float p)
        {
            return 1.0f - Mathf.Sqrt(1.0f - Mathf.Pow(v, p));
        }

        public static float EaseInOut(float v, float p)
        {
            return 
                v < 0.5f ? 
                Mathf.Pow(2, p - 1) * Mathf.Pow(v, p) : 
                1.0f - (Mathf.Pow(-2 * v + 2, p) / 2.0f);
        }

        public static float EaseInOutSine(float v, float p)
        {
            return Mathf.Pow(-(Mathf.Cos(PI * v) - 1) / 2.0f, p);
        }

        // Sin Functions
        public static float Sine(float v, float p)
        {
            return Mathf.Abs(Mathf.Pow(Mathf.Sin(PI * v), p));
        }

        public static float SineSqrt(float v, float p)
        {
            return Mathf.Sqrt(Mathf.Abs(Mathf.Pow(Mathf.Sin(PI * v), p)));
        }

        public static float RepeatedSine(float v, float p)
        {
            return
                v < 0.5f ?
                Mathf.Pow(Mathf.Sin(PI * v), p) / 2.0f :
                1.0f + (-Mathf.Pow(Mathf.Sin(PI * v), p) / 2.0f);
        }

        public static float SineFrequency(float v, float p)
        {
            return Mathf.Sin(PI * Mathf.Pow(v, p));
        }

        public static float RadianArcSine(float v, float p)
        {
            return Mathf.Pow(Mathf.Asin(v * Radian), p);
        }

        public static float RadianArcSineSqrt(float v, float p)
        {
            return Mathf.Sqrt(Mathf.Pow(Mathf.Asin(v * Radian), p));
        }

        public static float HalfDownUpSine(float v, float p)
        {
            return
                v < 0.5f ?
                0.5f + (-Mathf.Pow(Mathf.Sin(PI * v), p)) / 2.0f :
                1.0f + -Mathf.Pow(Mathf.Sin(PI * v), p);
        }
    }
}
