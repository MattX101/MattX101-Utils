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
        public float Roll => (_roll + ExternalRoll) % 360.0f;

        [NonSerialized]
        public float ExternalRoll;
        
        [Space]

        [SerializeField, Range(0, 1)]
        private float _minValue;
        public float MinValue => _minValue;

        [SerializeField, Range(0, 1)]
        private float _maxValue = 1.0f;
        public float MaxValue => _maxValue;

        [Space]

        [SerializeField]
        private float _zoomOut = 1.0f;
        public float ZoomOut => MathF.Abs(_zoomOut * ExternalZoomOut);

        [NonSerialized]
        public float ExternalZoomOut = 1.0f;
        
        [SerializeField]
        private Vector2 _scale = Vector2.one;
        public Vector2 Scale => _scale * ExternalScale;

        [NonSerialized]
        public Vector2 ExternalScale = Vector2.one;
        
        [SerializeField]
        private Vector2 _offset = Vector2.zero;
        public Vector2 Offset => _offset - ExternalOffset;
        
        [NonSerialized]
        public Vector2 ExternalOffset = Vector2.zero;

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
