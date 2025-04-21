using UnityEngine;

namespace Utils.Filters
{
    public static class Invert
    {
        public static ComputeShader Shader { get; set; }

        private static void Calculate(ref float v)
        {
            v = 1.0f - v;
        }

        private static void Calculate(ref Color c)
        {
            Calculate(ref c.r);
            Calculate(ref c.g);
            Calculate(ref c.b);
            c.a = 1.0f;
        }

        public static void Compute(ref float[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Calculate(ref values[i]);
            }
        }

        public static void Compute(ref Color[] colors)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                Calculate(ref colors[i]);
            }
        }

        public static void Compute(ref ComputeBuffer buffer, bool isFloat)
        {
            int kernel;

            switch (isFloat)
            {
                case false:
                    kernel = Shader.FindKernel("InvertColor");
                    Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID(("sourceColor")), buffer);
                    break;
                case true:
                    kernel = Shader.FindKernel("InvertFloat");
                    Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID(("sourceFloat")), buffer);
                    break;
            }

            Shader.Dispatch(kernel, Mathf.CeilToInt(buffer.count / 1024.0f), 1, 1);
        }
    }
}
