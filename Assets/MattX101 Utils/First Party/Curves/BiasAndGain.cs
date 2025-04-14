using UnityEngine;

namespace Utils.Curves
{
    public static partial class BiasAndGain
    {
        public static ComputeShader Shader
        {
            get;
            set;
        }

        public static float Bias(float time, float bias)
        {
            return time / (((1.0f / bias) - 2.0f) * (1.0f - time) + 1.0f);
        }

        public static float Gain(float time, float gain)
        {
            return
                time < 0.5f ?
                Bias(time * 2, gain) / 2 :
                Bias(time * 2 - 1, 1 - gain) / 2.0f + 0.5f;
        }

        public static void ModifyValues(ref ComputeBuffer valuesBuffer, float bias, float gain)
        {
            int kernel = Shader.FindKernel("Values");
            Shader.SetBuffer(kernel, "values", valuesBuffer);

            Shader.SetFloat("bias", bias);
            Shader.SetFloat("gain", gain);

            Shader.Dispatch(kernel, Mathf.CeilToInt(valuesBuffer.count / 1024.0f), 1, 1);
        }

        public static void ModifyImage(ref ComputeBuffer colorsBuffer, float bias, float gain)
        {
            int kernel = Shader.FindKernel("Texture");
            Shader.SetBuffer(kernel, "image", colorsBuffer);

            Shader.SetFloat("bias", bias);
            Shader.SetFloat("gain", gain);

            Shader.Dispatch(kernel, Mathf.CeilToInt(colorsBuffer.count / 1024.0f), 1, 1);
        }
    }
}
