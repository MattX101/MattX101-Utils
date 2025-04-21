using System;
using UnityEngine;
using Utils.Noise.Profiles;

namespace Utils.Mask
{
    [Serializable]
    public class MaskSettings
    {
        [SerializeField]
        private bool _invert;
        public bool Invert => _invert;
        
        [Space]

        [SerializeField]
        private float _power = 1;
        public float Power => _power;

        [Space]

        [SerializeField]
        private bool _computeRoll;
        public bool ComputeRoll => _computeRoll;

        [SerializeField, Range(0, 360)]
        private float _roll;
        public float Roll => (_roll + _externalRoll) % 360.0f;

        private float _externalRoll;
        public float ExternalRoll
        {
            set => _externalRoll = value;
        }
        
        [Space]

        [SerializeField, Range(0, 1)]
        private float _minValue;
        public float MinValue => _minValue;

        [SerializeField, Range(0, 1)]
        private float _maxValue = 1.0f;
        public float MaxValue => _maxValue;

        [Space]

        [SerializeField]
        private float _zoom = 1.0f;
        public float Zoom => MathF.Abs(_zoom * _externalZoom);

        private float _externalZoom = 1.0f;
        public float ExternalZoom
        {
            set => _externalZoom = value;
        }
        
        [SerializeField]
        private Vector2 _scale = Vector2.one;
        public Vector2 Scale => _scale * _externalScale;

        private Vector2 _externalScale = Vector2.one;
        public Vector2 ExternalScale
        {
            set => _externalScale = value;
        }
        
        [SerializeField]
        private Vector2 _offset = Vector2.zero;
        public Vector2 Offset => _offset - _externalOffset;
        
        private Vector2 _externalOffset = Vector2.zero;
        public Vector2 ExternalOffset
        {
            set => _externalOffset = value;
        }

        [Space]

        [Header("Noise")]

        [SerializeField]
        private bool _applyNoise = true;
        public bool ApplyNoise => _applyNoise;

        [SerializeField]
        private float _warp = 1.0f;
        public float Warp => Mathf.Abs(_warp);

        [SerializeField]
        private NoiseProfile _noiseProfile;
        public NoiseProfile NoiseProfile => _noiseProfile;

        [SerializeField]
        private WarpProfile _warpProfile;
        public WarpProfile WarpProfile => _warpProfile;
    }
}
