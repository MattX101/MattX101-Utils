using UnityEngine;

namespace Utils.Curves
{
    public static class CurvesGPU
    {
        public static ComputeShader Shader
        {
            get;
            set;
        }

        private static string GetEquation(int equation)
        {
            return equation switch
            {
                0 => "EaseIn",
                1 => "EaseInCirc",
                2 => "EaseInOut",
                3 => "EaseInOutSine",

                4 => "Sine",
                5 => "SineSqrt",
                6 => "RepeatedSine",
                7 => "SineFrequency",
                8 => "RadianArcSine",
                9 => "RadianArcSineSqrt",
                10 => "HalfDownUpSine",

                _ => "EaseIn"
            };
        }

        public static void ModifyValues(ref ComputeBuffer valuesBuffer, int equation, float power)
        {
            int kernel = Shader.FindKernel(GetEquation(equation) + "_Single");
            Shader.SetBuffer(kernel, "values", valuesBuffer);

            Shader.SetFloat("power", power);

            Shader.Dispatch(kernel, Mathf.CeilToInt(valuesBuffer.count / 1024.0f), 1, 1);
        }

        public static void ModifyImage(ref ComputeBuffer colorsBuffer, int equation, float power)
        {
            int kernel = Shader.FindKernel(GetEquation(equation) + "_Image");
            Shader.SetBuffer(kernel, "image", colorsBuffer);

            Shader.SetFloat("power", power);

            Shader.Dispatch(kernel, Mathf.CeilToInt(colorsBuffer.count / 1024.0f), 1, 1);
        }
    }
}
