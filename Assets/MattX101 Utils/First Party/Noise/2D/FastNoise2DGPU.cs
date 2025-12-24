using Utils.Noise.Profiles;
using UnityEngine;

namespace Utils.Noise
{
    public static partial class FastNoise2D
    {
        public static ComputeShader Shader { get; set; }

        public static void Generate(ref ComputeBuffer noiseBuffer, int width, int height, bool is3D, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            float sin = 0, cos = 0;
            CalculateAngles(ref sin, ref cos, noiseProfile.Roll);
            
            const float DefaultCanvasSize = 1000.0f;
            float canvasToScreenRatio = Mathf.Max(DefaultCanvasSize / width, DefaultCanvasSize / height);

            Shader.SetFloat(UnityEngine.Shader.PropertyToID("sin"), sin);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("cos"), cos);

            Shader.SetInt(UnityEngine.Shader.PropertyToID("width"), width);
            Shader.SetInt(UnityEngine.Shader.PropertyToID("height"), height);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("halfWidth"), width / 2.0f);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("halfHeight"), height / 2.0f);

            Shader.SetFloats(UnityEngine.Shader.PropertyToID("canvasToScreenRatio"), canvasToScreenRatio, canvasToScreenRatio);

            int kernel;
            if (noiseProfile.warp)
            {
                InitNoiseProfile(Shader, noiseProfile, canvasToScreenRatio);
                InitWarpProfile(Shader, warpProfile, canvasToScreenRatio);

                if (noiseProfile.normalized)
                {
                    if (noiseProfile.computeRoll)
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
                    if (noiseProfile.computeRoll)
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
                InitNoiseProfile(Shader, noiseProfile, canvasToScreenRatio);

                if (noiseProfile.normalized)
                {
                    if (noiseProfile.computeRoll)
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
                    if (noiseProfile.computeRoll)
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

        private static void InitNoiseProfile(ComputeShader noiseShader, NoiseProfile noiseProfile, float canvasToScreenRatio)
        {
            // Noise
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("seed"), noiseProfile.seed);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("frequency"), noiseProfile.Frequency);
            noiseShader.SetFloats(UnityEngine.Shader.PropertyToID("scale"), GetNoiseScaleX(noiseProfile, canvasToScreenRatio), GetNoiseScaleY(noiseProfile, canvasToScreenRatio));
            noiseShader.SetFloats(UnityEngine.Shader.PropertyToID("offset"), noiseProfile.Offset.x, noiseProfile.Offset.y, noiseProfile.Offset.z);
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("noiseType"), (int)noiseProfile.GetNoiseType(noiseProfile.type));

            // Fractal
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("fractalType"), (int)noiseProfile.GetFractalType(noiseProfile.fractal));
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("octaves"), noiseProfile.octaves);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("lacunarity"), noiseProfile.lacunarity);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("gain"), noiseProfile.gain);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("weightedStrength"), noiseProfile.weightedStrength);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("pingPong"), noiseProfile.pingPongStrength);

            // Cellular
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("cellularDistanceFunction"), (int)noiseProfile.cellularDistance);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("cellularReturnType"), (int)noiseProfile.cellularReturn);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("jitter"), noiseProfile.jitter);
        }

        private static void InitWarpProfile(ComputeShader noiseShader, WarpProfile warpProfile, float canvasToScreenRatio)
        {
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpAmp"), warpProfile.warpAmp);

            // Noise
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("warpSeed"), warpProfile.seed);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpFrequency"), warpProfile.Frequency);
            noiseShader.SetFloats(UnityEngine.Shader.PropertyToID("warpScale"), GetWarpScaleX(warpProfile, canvasToScreenRatio), GetWarpScaleY(warpProfile, canvasToScreenRatio));
            noiseShader.SetFloats(UnityEngine.Shader.PropertyToID("warpOffset"), warpProfile.Offset.x, warpProfile.Offset.y, warpProfile.Offset.z);
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("warpNoiseType"), (int)warpProfile.GetNoiseType(warpProfile.type));

            // Fractal
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("warpFractalType"), (int)warpProfile.GetFractalType(warpProfile.fractal));
            noiseShader.SetInt(UnityEngine.Shader.PropertyToID("warpOctaves"), warpProfile.octaves);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpLacunarity"), warpProfile.lacunarity);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpGain"), warpProfile.gain);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpWeightedStrength"), warpProfile.weightedStrength);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpPingPong"), warpProfile.pingPongStrength);

            // Cellular
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpCellularDistanceFunction"), (int)warpProfile.cellularDistance);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpCellularReturnType"), (int)warpProfile.cellularReturn);
            noiseShader.SetFloat(UnityEngine.Shader.PropertyToID("warpJitter"), warpProfile.jitter);
        }
    }
}
