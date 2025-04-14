using Utils.Filters.Blur;
using UnityEngine;

namespace Utils.Filters.Sharpen
{
    public static class BoxSharpen
    {
        private static void Sharpen(ref float filter, float source)
        {
            filter = 2.0f * source - filter;
        }

        private static void Sharpen(ref Color filter, Color source)
        {
            Sharpen(ref filter.r, source.r);
            Sharpen(ref filter.g, source.g);
            Sharpen(ref filter.b, source.b);
        }

        public static void Compute(ref float[] filter, float[] source, int width, int height)
        {
            BoxBlur.Compute(ref filter, source, width, height);

            for (int i = 0; i < source.Length; i++)
            {
                Sharpen(ref filter[i], source[i]);
            }
        }

        public static void Compute(ref Color[] filter, Color[] source, int width, int height)
        {
            BoxBlur.Compute(ref filter, source, width, height);

            for (int i = 0; i < source.Length; i++)
            {
                Sharpen(ref filter[i], source[i]);
            }
        }

        public static void Compute(ref ComputeBuffer filter, ComputeBuffer source, int width, int height, bool isFloat)
        {
            BoxBlur.Shader.SetInt("width", width);
            BoxBlur.Shader.SetInt("height", height);

            int kernel = BoxBlur.Shader.FindKernel(isFloat ? "SharpFloat" : "SharpColor");

            if (isFloat)
            {
                BoxBlur.Shader.SetBuffer(kernel, "sourceFloat", source);
                BoxBlur.Shader.SetBuffer(kernel, "filterFloat", filter);
            }
            else
            {
                BoxBlur.Shader.SetBuffer(kernel, "sourceColor", source);
                BoxBlur.Shader.SetBuffer(kernel, "filterColor", filter);
            }

            BoxBlur.Shader.Dispatch(kernel, Mathf.CeilToInt(width / 32.0f), Mathf.CeilToInt(height / 32.0f), 1);
        }
    }
}
