using UnityEngine;

namespace Utils.Colors.Coloring
{
    public static class Coloring
    {
        private const int InterpolationGradientPoints = 128;

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

        public static void ChannelColorigCPU(ref Color[] colors, Channel channel, Color source)
        {
            if (channel == Channel.Red)
            {
                for (int i = 0; i < colors.Length; i++)
                    colors[i] = new Color(colors[i].r * source.r, colors[i].r * source.g, colors[i].r * source.b, 1);
            }
            else if (channel == Channel.Green)
            {
                for (int i = 0; i < colors.Length; i++)
                    colors[i] = new Color(colors[i].g * source.r, colors[i].g * source.g, colors[i].g * source.b, 1);
            }
            else
            {
                for (int i = 0; i < colors.Length; i++)
                    colors[i] = new Color(colors[i].b * source.r, colors[i].b * source.g, colors[i].b * source.b, 1);
            }
        }

        public static void ChannelColorigGPU(ref ComputeBuffer colorsBuffer, Channel channel, Color source)
        {
            int kernel = 0;
            switch (channel)
            {
                case Channel.Red:
                    kernel = Shader.FindKernel("RedChannelColoring");
                    break;
                case Channel.Green:
                    kernel = Shader.FindKernel("GreenChannelColoring");
                    break;
                case Channel.Blue:
                    kernel = Shader.FindKernel("BlueChannelColoring");
                    break;
                default:
                    kernel = Shader.FindKernel("RedChannelColoring");
                    break;
            }

            Shader.SetBuffer(kernel, "colors", colorsBuffer);
            Shader.SetFloats("source", source.r, source.g, source.b, 1);
            Shader.Dispatch(kernel, Mathf.CeilToInt(colorsBuffer.count / 1024.0f), 1, 1);
        }

        public static void ColoringCPU(ref Color[] colors, float[] values, Color source)
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

        public static void ColoringGPU(ref ComputeBuffer colorsBuffer, ComputeBuffer valuesBuffer, Color source)
        {
            int kernel = Shader.FindKernel("Coloring");
            Shader.SetBuffer(kernel, "values", valuesBuffer);
            Shader.SetBuffer(kernel, "colors", colorsBuffer);

            Shader.SetFloats("source", source.r, source.g, source.b, 1);

            Shader.Dispatch(kernel, Mathf.CeilToInt(colorsBuffer.count / 1024.0f), 1, 1);
        }

        public static void GradientColoringCPU(ref Color[] colors, float[] values, Gradient gradient)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = gradient.Evaluate(values[i]);
            }
        }

        public static void GradientColoringGPU(ref ComputeBuffer colorsBuffer, ComputeBuffer valuesBuffer, Gradient gradient)
        {
            Vector4[] interpolatedGradient = InterpolateGradient(gradient);

            ComputeBuffer gradientBuffer = new ComputeBuffer(interpolatedGradient.Length, sizeof(float) * 4);
            gradientBuffer.SetData(interpolatedGradient);

            int kernel = Shader.FindKernel("GradientColoring");
            Shader.SetBuffer(kernel, "gradient", gradientBuffer);
            Shader.SetBuffer(kernel, "values", valuesBuffer);
            Shader.SetBuffer(kernel, "colors", colorsBuffer);

            Shader.SetInt("gradientLength", interpolatedGradient.Length - 1);

            Shader.Dispatch(kernel, Mathf.CeilToInt((float)colorsBuffer.count / 1024.0f), 1, 1);

            gradientBuffer.Release();
        }

        private static Vector4[] InterpolateGradient(Gradient gradient)
        {
            Vector4[] interpolation = new Vector4[InterpolationGradientPoints];

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
