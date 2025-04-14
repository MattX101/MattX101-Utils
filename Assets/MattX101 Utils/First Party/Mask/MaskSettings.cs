using UnityEngine;
using System;
using Utils.Noise.Profiles;

namespace Utils.Mask
{
    [Serializable]
    public class MaskSettings
    {
        [SerializeField]
        private bool _invert = false;
        public bool Invert => _invert;

        [Space]

        [SerializeField]
        private float _power = 1;
        public float Power => _power;

        [Space]

        [SerializeField]
        private bool _computeRoll = false;
        public bool ComputeRoll => _computeRoll;

        [SerializeField, Range(0, 360)]
        private float _roll = 0.0f;
        public float Roll => _roll;

        [Space]

        [SerializeField, Range(0, 1)]
        private float _minValue = 0.0f;
        public float MinValue => _minValue;

        [SerializeField, Range(0, 1)]
        private float _maxValue = 1.0f;
        public float MaxValue => _maxValue;

        [Space]

        [SerializeField]
        private float _zoom = 1.0f;
        public float Zoom => MathF.Abs(_zoom);
        
        [SerializeField]
        private Vector2 _scale = Vector2.one;
        public Vector2 Scale => _scale;

        [SerializeField]
        private Vector2 _offset = Vector2.zero;
        public Vector2 Offset => _offset;

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

        [SerializeField]
        private bool _previewNoise = false;
        public bool PreviewNoise => _previewNoise;
    }
}
