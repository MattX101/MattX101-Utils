using Utils.Noise.Library;
using UnityEngine;
using System;

namespace Utils.Noise.Profiles
{
    [Serializable]
    public class Profile
    {
        internal FastNoiseLite FastNoise { get; } = new();

        [SerializeField]
        private int _seed;
        public int Seed => _seed;

        public float Frequency => 0.01f;

        [SerializeField]
        private float _universalScale = 1.0f;
        public float UniversalScale => _universalScale * ExternalUniversalScale;

        [NonSerialized] 
        public float ExternalUniversalScale = 1.0f;
        
        [SerializeField]
        private Vector3 _scale = Vector3.one;
        public Vector3 Scale =>
            new(
                _scale.x * ExternalScale.x,
                _scale.y * ExternalScale.y,
                _scale.z * ExternalScale.z
            );
        
        [NonSerialized]
        public Vector3 ExternalScale = Vector3.one;

        [SerializeField]
        private Vector3 _offset = Vector3.zero;
        public Vector3 Offset => _offset - ExternalOffset;
        
        [NonSerialized]
        public Vector3 ExternalOffset = Vector3.zero;
        
        [Header("Fractal")]

        [SerializeField]
        private NoiseType _type = NoiseType.Perlin;
        public NoiseType Type => _type;
        public enum NoiseType
        {
            Perlin,
            Simplex,
            Value,
            Cellular
        }

        [SerializeField]
        private FractalType _fractal = FractalType.FBm;
        public FractalType Fractal => _fractal;
        public enum FractalType
        {
            FBm,
            Ridged,
            PingPong
        }

        [SerializeField, Range(1, 10)]
        private int _octaves = 3;
        public int Octaves => _octaves;

        [SerializeField, Range(1, 10)]
        private float _lacunarity = 2.0f;
        public float Lacunarity => _lacunarity;

        [SerializeField, Range(0, 1)]
        private float _gain = 0.5f;
        public float Gain => _gain;

        [SerializeField, Range(0, 1)]
        private float _weightedStrength;
        public float WeightedStrength => _weightedStrength;

        [SerializeField, Range(0.5f, 5)]
        private float _pingPongStrength = 2.0f;
        public float PingPongStrength => _pingPongStrength;

        [Header("Cellular")]

        [SerializeField]
        private FastNoiseLite.CellularReturnType _cellularReturn = FastNoiseLite.CellularReturnType.Distance;
        public FastNoiseLite.CellularReturnType CellularReturn => _cellularReturn;

        [SerializeField]
        private FastNoiseLite.CellularDistanceFunction _cellularDistance = FastNoiseLite.CellularDistanceFunction.Euclidean;
        public FastNoiseLite.CellularDistanceFunction CellularDistance => _cellularDistance;

        [SerializeField, Range(0, 1)] 
        private float _jitter = 1.0f;
        public float Jitter => _jitter;

        public void SetSeed(int seed)
        {
            _seed = seed;
        }
        
        // Noise Types
        internal FastNoiseLite.NoiseType GetNoiseType(NoiseType type)
        {
            return type switch
            {
                NoiseType.Perlin => FastNoiseLite.NoiseType.Perlin,
                NoiseType.Simplex => FastNoiseLite.NoiseType.OpenSimplex2S,
                NoiseType.Value => FastNoiseLite.NoiseType.Value,
                NoiseType.Cellular => FastNoiseLite.NoiseType.Cellular,
                _ => FastNoiseLite.NoiseType.Perlin
            };
        }

        public void SetNoiseType_Perlin() => _type = NoiseType.Perlin;
        public void SetNoiseType_Simplex() => _type = NoiseType.Simplex;
        public void SetNoiseType_Value() => _type = NoiseType.Value;
        public void SetNoiseType_Cellular() => _type = NoiseType.Cellular;

        // Fractal Types
        internal FastNoiseLite.FractalType GetFractalType(FractalType fractal)
        {
            return fractal switch
            {
                FractalType.FBm => FastNoiseLite.FractalType.FBm,
                FractalType.Ridged => FastNoiseLite.FractalType.Ridged,
                FractalType.PingPong => FastNoiseLite.FractalType.PingPong,
                _ => FastNoiseLite.FractalType.FBm
            };
        }

        public void SetFractalType_FBm() => _fractal = FractalType.FBm;
        public void SetFractalType_Ridged() => _fractal = FractalType.Ridged;
        public void SetFractalType_PingPong() => _fractal = FractalType.PingPong;

        // Cellular
        public void SetCellular_Cell() => _cellularReturn = FastNoiseLite.CellularReturnType.CellValue;
        public void SetCellular_Distance() => _cellularReturn = FastNoiseLite.CellularReturnType.Distance;
        public void SetCellular_Distance2() => _cellularReturn = FastNoiseLite.CellularReturnType.Distance2;
        public void SetCellular_Distance2Add() => _cellularReturn = FastNoiseLite.CellularReturnType.Distance2Add;
        public void SetCellular_Distance2Sub() => _cellularReturn = FastNoiseLite.CellularReturnType.Distance2Sub;
        public void SetCellular_Distance2Mul() => _cellularReturn = FastNoiseLite.CellularReturnType.Distance2Mul;
        public void SetCellular_Distance2Div() => _cellularReturn = FastNoiseLite.CellularReturnType.Distance2Div;

        // Cellular Distance Function
        public void SetCellularDistanceFunction_Euclidean() => _cellularDistance = FastNoiseLite.CellularDistanceFunction.Euclidean;
        public void SetCellularDistanceFunction_EuclideanSq() => _cellularDistance = FastNoiseLite.CellularDistanceFunction.EuclideanSq;
        public void SetCellularDistanceFunction_Manhattan() => _cellularDistance = FastNoiseLite.CellularDistanceFunction.Manhattan;
        public void SetCellularDistanceFunction_Hybrid() => _cellularDistance = FastNoiseLite.CellularDistanceFunction.Hybrid;

        public void AnimateOffset(Vector3 shift)
        {
            _offset += shift * Time.deltaTime;
        }
    }
}
