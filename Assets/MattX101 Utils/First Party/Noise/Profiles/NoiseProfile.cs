using UnityEngine;
using System;

namespace Utils.Noise.Profiles
{
    [Serializable]
    public sealed class NoiseProfile : Profile
    {
        [Header("Warp")]
        public bool warp = false;

        public void Init()
        {
            FastNoise.SetSeed(seed);
            FastNoise.SetFrequency(Frequency);
            FastNoise.SetNoiseType(type);

            // Fractal
            FastNoise.SetFractalType(fractal);
            FastNoise.SetFractalOctaves(octaves);
            FastNoise.SetFractalLacunarity(lacunarity);
            FastNoise.SetFractalGain(gain);
            FastNoise.SetFractalWeightedStrength(weightedStrength);
            FastNoise.SetFractalPingPongStrength(pingPongStrength);

            // Cellular
            FastNoise.SetCellularReturnType(cellularReturn);
            FastNoise.SetCellularDistanceFunction(cellularDistance);
            FastNoise.SetCellularJitter(jitter);
        }

        internal float GetNoise2D(float x, float y)
        {
            return (FastNoise.GetNoise(x, y) + 1) / 2;
        }

        internal float GetNoise3D(float x, float y, float z)
        {
            return (FastNoise.GetNoise(x, y, z) + 1) / 2;
        }
    }
}
