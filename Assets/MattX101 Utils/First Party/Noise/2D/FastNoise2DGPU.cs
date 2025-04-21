using UnityEngine;
using Utils.Noise.Profiles;

namespace Utils.Noise
{
    public static partial class FastNoise2D
    {
        public static ComputeShader Shader { get; set; }

        public static void Generate(ref ComputeBuffer noiseBuffer, int width, int height, bool is3D, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            CalculateAngles(noiseProfile.Roll);

            Init(width, height);

            Shader.SetFloat(UnityEngine.Shader.PropertyToID("sin"), _sin);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("cos"), _cos);
            
            Shader.SetInt(UnityEngine.Shader.PropertyToID("width"), _width);
            Shader.SetInt(UnityEngine.Shader.PropertyToID("height"), _height);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("halfWidth"), _halfWidth);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("halfHeight"), _halfHeight);
            
            Shader.SetFloats(UnityEngine.Shader.PropertyToID("canvasToScreenRatio"), _canvasToScreenRatio, _canvasToScreenRatio);

            int kernel;
            if (noiseProfile.Warp)
            {
                InitNoiseProfile(Shader, noiseProfile);
                InitWarpProfile(Shader, warpProfile);

                if (noiseProfile.Normalized)
                {
                    if (noiseProfile.ComputeRoll)
                    {
                        kernel = is3D ? Shader.FindKernel("WarpNoise3D_Linear_Roll") : Shader.FindKernel("WarpNoise2D_Linear_Roll");
                    }
                    else
                    {
                        kernel = is3D ? Shader.FindKernel("WarpNoise3D_Linear") : Shader.FindKernel("WarpNoise2D_Linear");
                    }
                }
                else
                {
                    if (noiseProfile.ComputeRoll)
                    {
                        kernel = is3D ? Shader.FindKernel("WarpNoise3D_Roll") : Shader.FindKernel("WarpNoise2D_Roll");
                    }
                    else
                    {
                        kernel = is3D ? Shader.FindKernel("WarpNoise3D") : Shader.FindKernel("WarpNoise2D");
                    }
                }
            }
            else
            {
                InitNoiseProfile(Shader, noiseProfile);

                if (noiseProfile.Normalized)
                {
                    if (noiseProfile.ComputeRoll)
                    {
                        kernel = is3D ? Shader.FindKernel("Noise3D_Linear_Roll") : Shader.FindKernel("Noise2D_Linear_Roll");
                    }
                    else
                    {
                        kernel = is3D ? Shader.FindKernel("Noise3D_Linear") : Shader.FindKernel("Noise2D_Linear");
                    }
                }
                else
                {
                    if (noiseProfile.ComputeRoll)
                    {
                        kernel = is3D ? Shader.FindKernel("Noise3D_Roll") : Shader.FindKernel("Noise2D_Roll");
                    }
                    else
                    {
                        kernel = is3D ? Shader.FindKernel("Noise3D") : Shader.FindKernel("Noise2D");
                    }
                }
            }

            Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("noise"), noiseBuffer);

            Shader.Dispatch(kernel, Mathf.CeilToInt(width / 32.0f), Mathf.CeilToInt(height / 32.0f), 1);
        }

        private static void InitNoiseProfile(ComputeShader noiseShader, NoiseProfile noiseProfile)
        {
            // Noise
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("seed"), noiseProfile.Seed);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("frequency"), noiseProfile.Frequency);
            noiseShader.SetFloats(UnityEngine.Shader.PropertyToID("scale"), GetNoiseScaleX(noiseProfile), GetNoiseScaleY(noiseProfile));
            noiseShader.SetFloats(UnityEngine.Shader.PropertyToID("offset"), noiseProfile.Offset.x, noiseProfile.Offset.y, noiseProfile.Offset.z);
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("noiseType"), (int)noiseProfile.GetNoiseType(noiseProfile.Type));

            // Fractal
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("fractalType"), (int)noiseProfile.GetFractalType(noiseProfile.Fractal));
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("octaves"), noiseProfile.Octaves);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("lacunarity"), noiseProfile.Lacunarity);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("gain"), noiseProfile.Gain);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("weightedStrength"), noiseProfile.WeightedStrength);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("pingPong"), noiseProfile.PingPongStrength);

            // Cellular
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("cellularDistanceFunction"), (int)noiseProfile.CellularDistance);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("cellularReturnType"), (int)noiseProfile.CellularReturn);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("jitter"), noiseProfile.Jitter);
        }

        private static void InitWarpProfile(ComputeShader noiseShader, WarpProfile warpProfile)
        {
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpAmp"), warpProfile.WarpAmp);

            // Noise
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("warpSeed"), warpProfile.Seed);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpFrequency"), warpProfile.Frequency);
            noiseShader.SetFloats(UnityEngine.Shader.PropertyToID("warpScale"), GetWarpScaleX(warpProfile), GetWarpScaleY(warpProfile));
            noiseShader.SetFloats(UnityEngine.Shader.PropertyToID("warpOffset"), warpProfile.Offset.x, warpProfile.Offset.y, warpProfile.Offset.z);
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("warpNoiseType"), (int)warpProfile.GetNoiseType(warpProfile.Type));

            // Fractal
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("warpFractalType"), (int)warpProfile.GetFractalType(warpProfile.Fractal));
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("warpOctaves"), warpProfile.Octaves);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpLacunarity"), warpProfile.Lacunarity);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpGain"), warpProfile.Gain);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpWeightedStrength"), warpProfile.WeightedStrength);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpPingPong"), warpProfile.PingPongStrength);

            // Cellular
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpCellularDistanceFunction"), (int)warpProfile.CellularDistance);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpCellularReturnType"), (int)warpProfile.CellularReturn);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpJitter"), warpProfile.Jitter);
        }
    }
}
