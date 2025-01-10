using UnityEngine;
using UnityEngine.UI;
using Utils.Noise.Profiles;

namespace Utils.Noise.Preview
{
    internal sealed class NoisePreview2D : MonoBehaviour
    {
        private FastNoise2D _noise;

        [Header("Shader"), SerializeField]
        private ComputeShader _noiseShader;

        [Header("Profiles")]
        [SerializeField] private NoiseProfile _noiseProfile;
        [SerializeField] private WarpProfile _warpProfile;

        [SerializeField]
        private bool _useGPU = false, _toggle3D = true, _puase = true;

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

        private float[] _noiseMap;
        private Color[] _colorMap;

        private Texture2D _texture;

        void Awake()
        {
            NoiseShader.Shader = _noiseShader;

            _camera = FindObjectOfType<Camera>();
            _noise = new FastNoise2D();
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
                _noiseProfile.Init();
                _warpProfile.Init();
            }
            else
            {
                _noiseProfile.Init();
            }

            _noise.Generate(ref _noiseMap, _cameraWidth, _cameraHeight, _noiseProfile, _warpProfile, _toggle3D, _useGPU);

            _colorMap = new Color[_cameraWidth * _cameraHeight];
            for (int i = 0; i < _colorMap.Length; i++)
            {
                _colorMap[i] = _colorPalette.Evaluate(_noiseMap[i]);
            }

            _texture = new Texture2D(_cameraWidth, _cameraHeight);
            _texture.wrapMode = TextureWrapMode.Clamp;
            _texture.filterMode = FilterMode.Point;
            _texture.SetPixels(_colorMap);
            _texture.Apply();

            _preview.texture = _texture;

            _noiseProfile.offset.z += _increment * Time.deltaTime;
            _warpProfile.offset.z -= _increment * Time.deltaTime;
        }
    }
}