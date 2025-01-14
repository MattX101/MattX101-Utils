using UnityEngine;

namespace Utils.Colors.Blend
{
    public static class MixGPU
    {
        public static ComputeShader Shader
        {
            get;
            set;
        }

        public static void Blend(ref ComputeBuffer resultBuffer, ComputeBuffer baseBuffer, ComputeBuffer blendBuffer, Blends blendMode)
        {
            int kernel = GetBlend(blendMode);

            Shader.SetBuffer(kernel, "result", resultBuffer);
            Shader.SetBuffer(kernel, "base", baseBuffer);
            Shader.SetBuffer(kernel, "blend", blendBuffer);

            Shader.Dispatch(kernel, Mathf.CeilToInt(resultBuffer.count / 1024.0f), 1, 1);
        }

        private static int GetBlend(Blends mode)
        {
            return mode switch
            {
                /// Arithmetic
                Blends.Add => Shader.FindKernel("Add"),
                Blends.Subtract => Shader.FindKernel("Subtract"),
                Blends.Multiply => Shader.FindKernel("Multiply"),
                Blends.Divide => Shader.FindKernel("Divide"),
                Blends.Average => Shader.FindKernel("Average"),

                /// Darken
                Blends.Darken => Shader.FindKernel("Darken"),
                Blends.ColorBurn => Shader.FindKernel("ColorBurn"),
                Blends.LinearBurn => Shader.FindKernel("LinearBurn"),
                Blends.GammaDark => Shader.FindKernel("GammaDark"),

                /// Lighten
                Blends.Lighten => Shader.FindKernel("Lighten"),
                Blends.Shine => Shader.FindKernel("Shine"),
                Blends.ColorDodge => Shader.FindKernel("ColorDodge"),
                Blends.Screen => Shader.FindKernel("Screen"),
                Blends.Overlay => Shader.FindKernel("Overlay"),
                Blends.SoftLight => Shader.FindKernel("SoftLight"),
                Blends.HardLight => Shader.FindKernel("HardLight"),
                Blends.LinearLight => Shader.FindKernel("LinearLight"),
                Blends.HardMix => Shader.FindKernel("HardMix"),
                Blends.Tint => Shader.FindKernel("Tint"),
                Blends.GammaLight => Shader.FindKernel("GammaLight"),
                Blends.GammaIllumination => Shader.FindKernel("GammaIllumination"),

                /// Other
                Blends.Exclusion => Shader.FindKernel("Exclusion"),
                Blends.Difference => Shader.FindKernel("Difference"),
                Blends.Negation => Shader.FindKernel("Negation"),

                /// Default
                _ => Shader.FindKernel("Add")
            };
        }
    }
}
