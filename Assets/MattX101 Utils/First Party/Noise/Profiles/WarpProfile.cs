using UnityEngine;
using System;

namespace Utils.Noise.Profiles
{
    [Serializable]
    public sealed class WarpProfile : Profile
    {
        [Header("Warp")]
        public float warpAmp = 1.0f;

        public void Init(NoiseProfile noiseProfile)
        {
            FastNoise.SetSeed(noiseProfile.seed);
            FastNoise.SetFrequency(0.01f);
            FastNoise.SetNoiseType(noiseProfile.type);

            // Fractal
            FastNoise.SetFractalType(noiseProfile.fractal);
            FastNoise.SetFractalOctaves(noiseProfile.octaves);
            FastNoise.SetFractalLacunarity(noiseProfile.lacunarity);
            FastNoise.SetFractalGain(noiseProfile.gain);
            FastNoise.SetFractalWeightedStrength(noiseProfile.weightedStregth);
            FastNoise.SetFractalPingPongStrength(noiseProfile.pingPongStregth);

            // Cellular
            FastNoise.SetCellularReturnType(cellularReturn);
            FastNoise.SetCellularDistanceFunction(noiseProfile.cellularDistance);
            FastNoise.SetCellularJitter(noiseProfile.jitter);

            // Warp
            if (noiseProfile.warp)
            {
                FastNoise.SetSeed(seed);
                FastNoise.SetFrequency(0.01f);
                FastNoise.SetNoiseType(type);

                // Fractal
                FastNoise.SetFractalType(fractal);
                FastNoise.SetFractalOctaves(octaves);
                FastNoise.SetFractalLacunarity(lacunarity);
                FastNoise.SetFractalGain(gain);
                FastNoise.SetFractalWeightedStrength(weightedStregth);
                FastNoise.SetFractalPingPongStrength(pingPongStregth);

                // Cellular
                FastNoise.SetCellularReturnType(cellularReturn);
                FastNoise.SetCellularDistanceFunction(cellularDistance);
                FastNoise.SetCellularJitter(jitter);

                // Warp
                FastNoise.SetDomainWarpAmp(warpAmp);
            }
        }

        internal float GetWarp2D(float x, float y)
        {
            return FastNoise.GetNoise(x, y) * warpAmp;
        }

        internal float GetWarp3D(float x, float y, float z)
        {
            return FastNoise.GetNoise(x, y, z) * warpAmp;
        }
    }
}
