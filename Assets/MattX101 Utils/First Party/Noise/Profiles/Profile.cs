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
        public int _seed = 0;
        public int Seed
        {
            get
            {
                return _seed;
            }
            set
            {
                _seed = value;
            }
        }

        public float Frequency => 0.01f;

        [SerializeField]
        public float _universalScale = 1.0f;
        public float UniversalScale
        {
            get
            {
                return _universalScale * ExternalUniversalScale;
            }
            set
            {
                _universalScale = value;
            }
        }

        [NonSerialized] 
        public float ExternalUniversalScale = 1.0f;
        
        [SerializeField]
        private Vector3 _scale = Vector3.one;
        public Vector3 Scale
        {
            get
            {
                return new
                (
                    _scale.x * ExternalScale.x,
                    _scale.y * ExternalScale.y,
                    _scale.z * ExternalScale.z
                );
            }
            set
            {
                _scale = value;
            }
        }
        
        [NonSerialized]
        public Vector3 ExternalScale = Vector3.one;

        [SerializeField]
        private Vector3 _offset = Vector3.zero;
        public Vector3 Offset
        {
            get
            {
                return _offset - ExternalOffset;
            }
            set
            {
                _offset = value;
            }
        }
        
        [NonSerialized]
        public Vector3 ExternalOffset = Vector3.zero;
        
        [Header("Fractal")]

        public NoiseType type = NoiseType.Perlin;
        public enum NoiseType
        {
            Perlin,
            Simplex,
            Value,
            Cellular
        }

        public FractalType fractal = FractalType.FBm;
        public enum FractalType
        {
            FBm,
            Ridged,
            PingPong
        }

        [Range(1, 10)]
        public int octaves = 3;

        [Range(1, 10)]
        public float lacunarity = 2.0f;

        [Range(0, 1)]
        public float gain = 0.5f;

        [Range(0, 1)]
        public float weightedStrength;

        [Range(0.5f, 5)]
        public float pingPongStrength = 2.0f;

        [Header("Cellular")]

        public FastNoiseLite.CellularReturnType cellularReturn = FastNoiseLite.CellularReturnType.Distance;
        public FastNoiseLite.CellularDistanceFunction cellularDistance = FastNoiseLite.CellularDistanceFunction.Euclidean;

        [Range(0, 1)] 
        public float jitter = 1.0f;
        
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

        public void SetNoiseType_Perlin() => type = NoiseType.Perlin;
        public void SetNoiseType_Simplex() => type = NoiseType.Simplex;
        public void SetNoiseType_Value() => type = NoiseType.Value;
        public void SetNoiseType_Cellular() => type = NoiseType.Cellular;

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

        public void SetFractalType_FBm() => fractal = FractalType.FBm;
        public void SetFractalType_Ridged() => fractal = FractalType.Ridged;
        public void SetFractalType_PingPong() => fractal = FractalType.PingPong;

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

        public void AnimateOffset(Vector3 shift)
        {
            _offset += shift * Time.deltaTime;
        }
    }
}
