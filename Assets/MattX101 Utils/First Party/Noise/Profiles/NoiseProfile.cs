using UnityEngine;
using System;

namespace Utils.Noise.Profiles
{
    [Serializable]
    public sealed class NoiseProfile : Profile
    {
        [Space]

        public bool normalized;

        [Space]

        [Header("Roll")]
        public bool computeRoll;

        [SerializeField, Range(0, 360)]
        private float _roll;
        public float Roll
        {
            get
            {
                return (_roll + ExternalRoll) % 360.0f;
            }
            set
            {
                _roll = value;
            }
        }

        [NonSerialized]
        public float ExternalRoll;

        [Header("Warp")]
        public bool warp;

        public void Init()
        {
            FastNoise.SetSeed(seed);
            FastNoise.SetFrequency(Frequency);
            FastNoise.SetNoiseType(GetNoiseType(type));

            // Fractal
            FastNoise.SetFractalType(GetFractalType(fractal));
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
            return FastNoise.GetNoise(x, y);
        }

        internal float GetNoise3D(float x, float y, float z)
        {
            return FastNoise.GetNoise(x, y, z);
        }

        internal float GetNoise2D_Linear(float x, float y)
        {
            return (GetNoise2D(x, y) + 1) / 2;
        }

        internal float GetNoise3D_Linear(float x, float y, float z)
        {
            return (GetNoise3D(x, y, z) + 1) / 2;
        }
    }
}
