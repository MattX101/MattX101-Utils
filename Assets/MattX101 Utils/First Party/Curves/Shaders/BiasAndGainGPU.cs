using UnityEngine;

namespace Utils.Curves
{
    public static class BiasAndGainGPU
    {
        public static ComputeShader Shader
        {
            get;
            set;
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