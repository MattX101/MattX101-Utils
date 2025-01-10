using UnityEngine;
using System;

namespace Utils.Noise.Profiles
{
    [Serializable]
    public sealed class WarpProfile : Profile
    {
        [Header("Warp")]
        public float warpAmp = 1.0f;

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
