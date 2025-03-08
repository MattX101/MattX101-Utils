using UnityEngine;
using System;

namespace Utils.Noise.Profiles
{
    [Serializable]
    public sealed class WarpProfile : Profile
    {
        [Header("Warp")]

        [SerializeField]
        private float _warpAmp = 1.0f;
        public float WarpAmp => _warpAmp;

        public void Init()
        {
            FastNoise.SetSeed(Seed);
            FastNoise.SetFrequency(Frequency);
            FastNoise.SetNoiseType(GetNoiseType(Type));

            // Fractal
            FastNoise.SetFractalType(GetFractalType(Fractal));
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

        internal float GetWarp2D(float x, float y)
        {
            return FastNoise.GetNoise(x, y) * WarpAmp;
        }

        internal float GetWarp3D(float x, float y, float z)
        {
            return FastNoise.GetNoise(x, y, z) * WarpAmp;
        }
    }
}
