using UnityEngine;

namespace Utils.Colors.Coloring
{
    public static class Coloring
    {
        public static ComputeShader Shader
        {
            get;
            set;
        }

        public enum Channel
        {
            Red,
            Green,
            Blue
        }

        public static void Color(ref Color[] colors, float[] values, Color source)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(
                    values[i] * source.r,
                    values[i] * source.g,
                    values[i] * source.b,
                    1);
            }
        }

        public static void Color(ref ComputeBuffer colorsBuffer, ComputeBuffer valuesBuffer, Color source)
        {
            int kernel = Shader.FindKernel("Coloring");
            Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("values"), valuesBuffer);
            Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("colors"), colorsBuffer);

            Shader.SetFloats(UnityEngine.Shader.PropertyToID("source"), source.r, source.g, source.b, 1);

            Shader.Dispatch(kernel, Mathf.CeilToInt(colorsBuffer.count / 1024.0f), 1, 1);
        }

        public static void ChannelColoring(ref Color[] colors, Channel channel, Color source)
        {
            if (channel == Channel.Red)
            {
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = new Color(colors[i].r * source.r, colors[i].r * source.g, colors[i].r * source.b, 1);
                }
            }
            else if (channel == Channel.Green)
            {
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = new Color(colors[i].g * source.r, colors[i].g * source.g, colors[i].g * source.b, 1);
                }
            }
            else
            {
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = new Color(colors[i].b * source.r, colors[i].b * source.g, colors[i].b * source.b, 1);
                }
            }
        }

        public static void ChannelColoring(ref ComputeBuffer colorsBuffer, Channel channel, Color source)
        {
            int kernel = channel switch
            {
                Channel.Red => Shader.FindKernel("RedChannelColoring"),
                Channel.Green => Shader.FindKernel("GreenChannelColoring"),
                Channel.Blue => Shader.FindKernel("BlueChannelColoring"),
                _ => Shader.FindKernel("RedChannelColoring")
            };

            Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("colors"), colorsBuffer);
            Shader.SetFloats(UnityEngine.Shader.PropertyToID("source"), source.r, source.g, source.b, 1);

            Shader.Dispatch(kernel, Mathf.CeilToInt(colorsBuffer.count / 1024.0f), 1, 1);
        }

        public static void GradientColoring(ref Color[] colors, float[] values, Gradient gradient)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = gradient.Evaluate(values[i]);
            }
        }

        public static void GradientColoring(ref ComputeBuffer colorsBuffer, ComputeBuffer valuesBuffer, Gradient gradient)
        {
            const int InterpolationGradientPoints = 128;
            ComputeBuffer gradientBuffer = new ComputeBuffer(InterpolationGradientPoints, sizeof(float) * 4);
            gradientBuffer.SetData(InterpolateGradient(gradient, InterpolationGradientPoints));

            int kernel = Shader.FindKernel("GradientColoring");
            Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("gradient"), gradientBuffer);
            Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("values"), valuesBuffer);
            Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("colors"), colorsBuffer);

            Shader.SetInt(UnityEngine.Shader.PropertyToID("gradientLength"), InterpolationGradientPoints - 1);

            Shader.Dispatch(kernel, Mathf.CeilToInt(colorsBuffer.count / 1024.0f), 1, 1);

            gradientBuffer.Release();
        }

        private static Vector4[] InterpolateGradient(Gradient gradient, int length)
        {
            Vector4[] interpolation = new Vector4[length];

            for (int i = 0; i < interpolation.Length; i++)
            {
                float time = (float)i / (interpolation.Length - 1);
                Color colour = gradient.Evaluate(time);

                interpolation[i].x = colour.r;
                interpolation[i].y = colour.g;
                interpolation[i].z = colour.b;
                interpolation[i].w = time;
            }

            return interpolation;
        }
    }
}
