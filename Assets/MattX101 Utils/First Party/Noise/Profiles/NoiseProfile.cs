using UnityEngine;
using System;

namespace Utils.Noise.Profiles
{
    [Serializable]
    public sealed class NoiseProfile : Profile
    {
        [Header("Warp")]

        [SerializeField]
        private bool _warp = false;
        public bool Warp => _warp;

        public void Init()
        {
            FastNoise.SetSeed(Seed);
            FastNoise.SetFrequency(Frequency);
            FastNoise.SetNoiseType(Type);

            // Fractal
            FastNoise.SetFractalType(Fractal);
            FastNoise.SetFractalOctaves(Octaves);
            FastNoise.SetFractalLacunarity(Lacunarity);
            FastNoise.SetFractalGain(Gain);
            FastNoise.SetFractalWeightedStrength(WeightedStrength);
            FastNoise.SetFractalPingPongStrength(PingPongStrength);

            // Cellular
            FastNoise.SetCellularReturnType(CellularReturn);
            FastNoise.SetCellularDistanceFunction(CellularDistance);
            FastNoise.SetCellularJitter(Jitter);
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
