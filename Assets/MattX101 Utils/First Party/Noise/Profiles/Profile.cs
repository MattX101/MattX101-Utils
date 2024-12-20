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

        // Noise Types
        public void SetNoiseType_Perlin() => type = FastNoiseLite.NoiseType.Perlin;
        public void SetNoiseType_OpenSimplex() => type = FastNoiseLite.NoiseType.OpenSimplex2;
        public void SetNoiseType_OpenSimplexS() => type = FastNoiseLite.NoiseType.OpenSimplex2S;
        public void SetNoiseType_Value() => type = FastNoiseLite.NoiseType.Value;
        public void SetNoiseType_ValueCubic() => type = FastNoiseLite.NoiseType.ValueCubic;
        public void SetNoiseType_Cellular() => type = FastNoiseLite.NoiseType.Cellular;

        // Fractal Types
        public void SetFractalType_FBm() => fractal = FastNoiseLite.FractalType.FBm;
        public void SetFractalType_Ridged() => fractal = FastNoiseLite.FractalType.Ridged;
        public void SetFractalType_PingPong() => fractal = FastNoiseLite.FractalType.PingPong;

        // Cellular
        public void SetCellular_Cell() => cellularReturn = FastNoiseLite.CellularReturnType.CellValue;
        public void SetCellular_Distance() => cellularReturn = FastNoiseLite.CellularReturnType.Distance;
        public void SetCellular_Distance2() => cellularReturn = FastNoiseLite.CellularReturnType.Distance2;
        public void SetCellular_Distance2Add() => cellularReturn = FastNoiseLite.CellularReturnType.Distance2Add;
        public void SetCellular_Distance2Sub() => cellularReturn = FastNoiseLite.CellularReturnType.Distance2Sub;
        public void SetCellular_Distance2Mul() => cellularReturn = FastNoiseLite.CellularReturnType.Distance2Mul;
        public void SetCellular_Distance2Div() => cellularReturn = FastNoiseLite.CellularReturnType.Distance2Div;

        // Cellular Distance Function
        public void SetCellularDistanceFunction_Euclidean() => cellularDistance = FastNoiseLite.CellularDistanceFunction.Euclidean;
        public void SetCellularDistanceFunction_EuclideanSq() => cellularDistance = FastNoiseLite.CellularDistanceFunction.EuclideanSq;
        public void SetCellularDistanceFunction_Manhattan() => cellularDistance = FastNoiseLite.CellularDistanceFunction.Manhattan;
        public void SetCellularDistanceFunction_Hybrid() => cellularDistance = FastNoiseLite.CellularDistanceFunction.Hybrid;
    }
}
