using Utils.Noise.Library;
using UnityEngine;
using System;

namespace Utils.Noise.Profiles
{
    [Serializable]
    public class Profile
    {
        private FastNoiseLite _fastNoise = new FastNoiseLite();
        internal FastNoiseLite FastNoise => _fastNoise;

        public int seed = 0;

        public float universalScale = 1.0f;
        public Vector3 scale = new Vector3(1, 1, 1);

        public Vector3 offset = new Vector3(0, 0, 0);

        [Header("Fractal")]
        public FastNoiseLite.NoiseType type = FastNoiseLite.NoiseType.Perlin;
        public FastNoiseLite.FractalType fractal = FastNoiseLite.FractalType.FBm;

        public int octaves = 3;
        public float lacunarity = 2.0f;
        public float gain = 0.5f;
        public float weightedStregth = 0.0f;
        public float pingPongStregth = 2.0f;

        [Header("Cellular")]
        public FastNoiseLite.CellularReturnType cellularReturn = FastNoiseLite.CellularReturnType.Distance;
        public FastNoiseLite.CellularDistanceFunction cellularDistance = FastNoiseLite.CellularDistanceFunction.Euclidean;
        public float jitter = 1.0f;
    }
}
