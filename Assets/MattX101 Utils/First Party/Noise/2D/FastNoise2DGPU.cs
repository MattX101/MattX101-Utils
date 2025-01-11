using UnityEngine;
using Utils.Noise.Profiles;

namespace Utils.Noise
{
    public static class FastNoise2DGPU
    {
        private const float DefaultCanvasSize = 1000.0f;

        public static ComputeShader Shader
        {
            get;
            set;
        }

        public static void GenerateNoise(ref ComputeBuffer noiseBuffer, int resX, int resY, bool is3D, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            float ratioX = DefaultCanvasSize / (float)resX;
            float ratioY = DefaultCanvasSize / (float)resY;

            Shader.SetInt("resX", resX);
            Shader.SetFloat("resY", resY);
            Shader.SetFloat("halfResX", (float)resX / 2.0f);
            Shader.SetFloat("halfResY", (float)resY / 2.0f);

            int kernel = 0;
            if (noiseProfile.warp)
            {
                InitNoiseProfile(Shader, noiseProfile, ratioX, ratioY);
                InitWarpProfile(Shader, warpProfile, ratioX, ratioY);

                kernel = is3D ? Shader.FindKernel("WarpNoise3D") : Shader.FindKernel("WarpNoise2D");
            }
            else
            {
                InitNoiseProfile(Shader, noiseProfile, ratioX, ratioY);

                kernel = is3D ? Shader.FindKernel("Noise3D") : Shader.FindKernel("Noise2D");
            }

            Shader.SetBuffer(kernel, "noise", noiseBuffer);

            Shader.Dispatch(
                kernel,
                Mathf.CeilToInt(resX / 32.0f),
                Mathf.CeilToInt(resY / 32.0f),
                1);
        }

        private static void InitNoiseProfile(ComputeShader noiseShader, NoiseProfile noiseProfile, float ratioX, float ratioY)
        {
            // Noise
            noiseShader.SetInt("seed", noiseProfile.seed);
            noiseShader.SetFloat("frequency", noiseProfile.Frequency);
            noiseShader.SetFloats(
                "scale", 
                noiseProfile.scale.x / noiseProfile.universalScale / ratioX, 
                noiseProfile.scale.y / noiseProfile.universalScale / ratioY);
            noiseShader.SetFloats("offset", noiseProfile.offset.x, noiseProfile.offset.y, noiseProfile.offset.z);
            noiseShader.SetInt("noiseType", (int)noiseProfile.type);

            // Fractal
            noiseShader.SetInt("fractalType", (int)noiseProfile.fractal);
            noiseShader.SetInt("octaves", noiseProfile.octaves);
            noiseShader.SetFloat("lacunarity", noiseProfile.lacunarity);
            noiseShader.SetFloat("gain", noiseProfile.gain);
            noiseShader.SetFloat("weightedStrength", noiseProfile.weightedStrength);
            noiseShader.SetFloat("pingPong", noiseProfile.pingPongStrength);

            // Cellular
            noiseShader.SetFloat("cellularDistanceFunction", (int)noiseProfile.cellularDistance);
            noiseShader.SetFloat("cellularReturnType", (int)noiseProfile.cellularReturn);
            noiseShader.SetFloat("jitter", noiseProfile.jitter);
        }

        private static void InitWarpProfile(ComputeShader noiseShader, WarpProfile warpProfile, float ratioX, float ratioY)
        {
            noiseShader.SetFloat("warpAmp", warpProfile.warpAmp);

            // Noise
            noiseShader.SetInt("warpSeed", warpProfile.seed);
            noiseShader.SetFloat("warpFrequency", warpProfile.Frequency);
            noiseShader.SetFloats(
                "warpScale", 
                warpProfile.scale.x / warpProfile.universalScale / ratioX, 
                warpProfile.scale.y / warpProfile.universalScale / ratioY);
            noiseShader.SetFloats("warpOffset", warpProfile.offset.x, warpProfile.offset.y, warpProfile.offset.z);
            noiseShader.SetInt("warpNoiseType", (int)warpProfile.type);

            // Fractal
            noiseShader.SetInt("warpFractalType", (int)warpProfile.fractal);
            noiseShader.SetInt("warpOctaves", warpProfile.octaves);
            noiseShader.SetFloat("warpLacunarity", warpProfile.lacunarity);
            noiseShader.SetFloat("warpGain", warpProfile.gain);
            noiseShader.SetFloat("warpWeightedStrength", warpProfile.weightedStrength);
            noiseShader.SetFloat("warpPingPong", warpProfile.pingPongStrength);

            // Cellular
            noiseShader.SetFloat("warpCellularDistanceFunction", (int)warpProfile.cellularDistance);
            noiseShader.SetFloat("warpCellularReturnType", (int)warpProfile.cellularReturn);
            noiseShader.SetFloat("warpJitter", warpProfile.jitter);
        }
    }
}
