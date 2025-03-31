using UnityEngine;
using Utils.Noise.Profiles;

namespace Utils.Noise
{
    public static partial class FastNoise2D
    {
        public static ComputeShader Shader
        {
            get;
            set;
        }

        public static void Generate(ref ComputeBuffer noiseBuffer, int width, int height, bool is3D, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            Init(width, height);

            Shader.SetInt("width", _width);
            Shader.SetInt("height", _height);
            Shader.SetFloat("halfWidth", _halfWidth);
            Shader.SetFloat("halfHeight", _halfHeight);
            Shader.SetFloats("canvasToScreenRatio", CanvasToScreenRatio, CanvasToScreenRatio);

            int kernel = 0;
            if (noiseProfile.Warp)
            {
                InitNoiseProfile(Shader, noiseProfile);
                InitWarpProfile(Shader, warpProfile);

                if (noiseProfile.Normalized)
                {
                    kernel = is3D ? Shader.FindKernel("WarpNoise3D_Linear") : Shader.FindKernel("WarpNoise2D_Linear");
                }
                else
                {
                    kernel = is3D ? Shader.FindKernel("WarpNoise3D") : Shader.FindKernel("WarpNoise2D");
                }
            }
            else
            {
                InitNoiseProfile(Shader, noiseProfile);

                if (noiseProfile.Normalized)
                {
                    kernel = is3D ? Shader.FindKernel("Noise3D_Linear") : Shader.FindKernel("Noise2D_Linear");
                }
                else
                {
                    kernel = is3D ? Shader.FindKernel("Noise3D") : Shader.FindKernel("Noise2D");
                }
            }

            Shader.SetBuffer(kernel, "noise", noiseBuffer);

            Shader.Dispatch(kernel, Mathf.CeilToInt(width / 32.0f), Mathf.CeilToInt(height / 32.0f), 1);
        }

        private static void InitNoiseProfile(ComputeShader noiseShader, NoiseProfile noiseProfile)
        {
            // Noise
            noiseShader.SetInt("seed", noiseProfile.Seed);
            noiseShader.SetFloat("frequency", noiseProfile.Frequency);
            noiseShader.SetFloats("scale", GetNoiseScaleX(noiseProfile), GetNoiseScaleY(noiseProfile));
            noiseShader.SetFloats("offset", noiseProfile.Offset.x, noiseProfile.Offset.y, noiseProfile.Offset.z);
            noiseShader.SetInt("noiseType", (int)noiseProfile.GetNoiseType(noiseProfile.Type));

            // Fractal
            noiseShader.SetInt("fractalType", (int)noiseProfile.GetFractalType(noiseProfile.Fractal));
            noiseShader.SetInt("octaves", noiseProfile.Octaves);
            noiseShader.SetFloat("lacunarity", noiseProfile.Lacunarity);
            noiseShader.SetFloat("gain", noiseProfile.Gain);
            noiseShader.SetFloat("weightedStrength", noiseProfile.WeightedStrength);
            noiseShader.SetFloat("pingPong", noiseProfile.PingPongStrength);

            // Cellular
            noiseShader.SetFloat("cellularDistanceFunction", (int)noiseProfile.CellularDistance);
            noiseShader.SetFloat("cellularReturnType", (int)noiseProfile.CellularReturn);
            noiseShader.SetFloat("jitter", noiseProfile.Jitter);
        }

        private static void InitWarpProfile(ComputeShader noiseShader, WarpProfile warpProfile)
        {
            noiseShader.SetFloat("warpAmp", warpProfile.WarpAmp);

            // Noise
            noiseShader.SetInt("warpSeed", warpProfile.Seed);
            noiseShader.SetFloat("warpFrequency", warpProfile.Frequency);
            noiseShader.SetFloats("warpScale", GetWarpScaleX(warpProfile), GetWarpScaleY(warpProfile));
            noiseShader.SetFloats("warpOffset", warpProfile.Offset.x, warpProfile.Offset.y, warpProfile.Offset.z);
            noiseShader.SetInt("warpNoiseType", (int)warpProfile.GetNoiseType(warpProfile.Type));

            // Fractal
            noiseShader.SetInt("warpFractalType", (int)warpProfile.GetFractalType(warpProfile.Fractal));
            noiseShader.SetInt("warpOctaves", warpProfile.Octaves);
            noiseShader.SetFloat("warpLacunarity", warpProfile.Lacunarity);
            noiseShader.SetFloat("warpGain", warpProfile.Gain);
            noiseShader.SetFloat("warpWeightedStrength", warpProfile.WeightedStrength);
            noiseShader.SetFloat("warpPingPong", warpProfile.PingPongStrength);

            // Cellular
            noiseShader.SetFloat("warpCellularDistanceFunction", (int)warpProfile.CellularDistance);
            noiseShader.SetFloat("warpCellularReturnType", (int)warpProfile.CellularReturn);
            noiseShader.SetFloat("warpJitter", warpProfile.Jitter);
        }
    }
}
