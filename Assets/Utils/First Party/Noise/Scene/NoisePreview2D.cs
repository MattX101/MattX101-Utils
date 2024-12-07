# if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using Utils.Noise.Profiles;

namespace Utils.Noise.Preview
{
    internal class NoisePreview2D : MonoBehaviour
    {
        [Header("Profiles")]
        [SerializeField] private NoiseProfile _noiseProfile;
        [SerializeField] private WarpProfile _warpProfile;

        [SerializeField]
        private bool _toggle3D = true, _puase = true;

        [Space]

        [SerializeField]
        private float _increment = 1.0f;

        [SerializeField, Range(1, 16)]
        private int _downScaler = 16;

        [Space]

        [SerializeField]
        public Gradient _colorPalette;

        [SerializeField]
        private RawImage _preview;

        private Camera _camera;
        private int _cameraWidth, _cameraHeight;

        private FastNoise2D _fastNoise;

        private float[] _noiseMap;
        private Color[] _colorMap;

        private Texture2D _texture;

        void Awake()
        {
            _camera = FindObjectOfType<Camera>();
            _fastNoise = new FastNoise2D();
        }

        private void Update()
        {
            if (_puase)
                return;

            _cameraWidth = _camera.pixelWidth / _downScaler;
            _cameraHeight = _camera.pixelHeight / _downScaler;

            _noiseMap = new float[_cameraWidth * _cameraHeight];

            if (_noiseProfile.warp)
            {
                _warpProfile.Init(_noiseProfile);

                if (_toggle3D)
                {
                    _fastNoise.WarpedNoise3DToMap2D(ref _noiseMap, _cameraWidth, _cameraHeight, _noiseProfile, _warpProfile);
                }
                else
                {
                    _fastNoise.WarpedNoise2DToMap2D(ref _noiseMap, _cameraWidth, _cameraHeight, _noiseProfile, _warpProfile);
                }
            }
            else
            {
                _noiseProfile.Init();

                if (_toggle3D)
                {
                    _fastNoise.Noise3DToMap2D(ref _noiseMap, _cameraWidth, _cameraHeight, _noiseProfile);
                }
                else
                {
                    _fastNoise.Noise2DToMap2D(ref _noiseMap, _cameraWidth, _cameraHeight, _noiseProfile);
                }
            }

            _colorMap = new Color[_cameraWidth * _cameraHeight];
            for (int y = 0, i = 0; y < _cameraHeight; y++)
            {
                for (int x = 0; x < _cameraWidth; x++, i++)
                {
                    _colorMap[i] = _colorPalette.Evaluate(_noiseMap[i]);
                }
            }

            _texture = new Texture2D(_cameraWidth, _cameraHeight);
            _texture.filterMode = FilterMode.Point;
            _texture.wrapMode = TextureWrapMode.Clamp;
            _texture.SetPixels(_colorMap);
            _texture.Apply();

            _preview.texture = _texture;

            _noiseProfile.offset.z += _increment * Time.deltaTime;
            _warpProfile.offset.z -= _increment * Time.deltaTime;
        }
    }
}
# endif