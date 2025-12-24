using Utils.Noise.Profiles;
using UnityEngine;
using System;

namespace Utils.Mask
{
    [Serializable]
    public class MaskSettings
    {
        public bool invert;
        
        [Space]

        public float power = 1;

        [Space]

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
        
        [Space]

        [Range(0, 1)]
        public float minValue = 0.0f, maxValue = 1.0f;

        [Space]

        [SerializeField]
        private float _zoomOut = 1.0f;
        public float ZoomOut
        {
            get
            {
                return MathF.Abs(_zoomOut * ExternalZoomOut);;
            }
            set
            {
                _zoomOut = value;
            }
        }

        [NonSerialized]
        public float ExternalZoomOut = 1.0f;
        
        [SerializeField]
        private Vector2 _scale = Vector2.one;
        public Vector2 Scale
        {
            get
            {
                return _scale * ExternalScale;
            }
            set
            {
                _scale = value;
            }
        }

        [NonSerialized]
        public Vector2 ExternalScale = Vector2.one;
        
        [SerializeField]
        private Vector2 _offset = Vector2.zero;
        public Vector2 Offset
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
        public Vector2 ExternalOffset = Vector2.zero;

        [Space]

        [Header("Noise")]

        public bool applyNoise = true;

        [SerializeField]
        private float _warp = 1.0f;
        public float Warp
        {
            get
            {
                return Mathf.Abs(_warp);
            }
            set
            {
                _warp = value;
            }
        }

        public NoiseProfile noiseProfile;
        public WarpProfile warpProfile;
    }
}
