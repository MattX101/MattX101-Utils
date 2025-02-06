using Utils.Filters.Blur;
using UnityEngine;

namespace Utils.Filters.Sharpen
{
    public static class BoxSharpen
    {
        private static void SharpenFloat(ref float filter, float source)
        {
            filter = 2.0f * source - filter;
        }

        private static void SharpenColor(ref Color filter, Color source)
        {
            SharpenFloat(ref filter.r, source.r);
            SharpenFloat(ref filter.g, source.g);
            SharpenFloat(ref filter.b, source.b);
        }

        public static void SharpenFloat(ref float[] filter, float[] source, int width, int height)
        {
            BoxBlur.BlurFloat(ref filter, source, width, height);

            for (int i = 0; i < source.Length; i++)
            {
                SharpenFloat(ref filter[i], source[i]);
            }
        }

        public static void SharpenColor(ref Color[] filter, Color[] source, int width, int height)
        {
            BoxBlur.BlurColor(ref filter, source, width, height);

            for (int i = 0; i < source.Length; i++)
            {
                SharpenColor(ref filter[i], source[i]);
            }
        }

        public static void SharpenFloat(ref ComputeBuffer filter, ComputeBuffer source, int width, int height)
        {
            DispatchShader(ref filter, source, width, height, true);
        }

        public static void SharpenColor(ref ComputeBuffer filter, ComputeBuffer source, int width, int height)
        {
            DispatchShader(ref filter, source, width, height, false);
        }

        private static void DispatchShader(ref ComputeBuffer filter, ComputeBuffer source, int width, int height, bool isFloat)
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

            BoxBlur.Shader.Dispatch(
                kernel,
                Mathf.CeilToInt(width / 32.0f),
                Mathf.CeilToInt(height / 32.0f),
                1);
        }
    }
}
