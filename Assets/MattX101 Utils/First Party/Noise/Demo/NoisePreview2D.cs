using UnityEngine;
using UnityEngine.UI;
using Utils.Noise.Profiles;

namespace Utils.Noise.Preview
{
    internal sealed class NoisePreview2D : MonoBehaviour
    {
        [Header("Profiles")]
        [SerializeField] private NoiseProfile _noiseProfile;
        [SerializeField] private WarpProfile _warpProfile;

        [Space]

        [SerializeField] private bool _useGPU;
        [SerializeField] private bool _toggle3D = true;
        [SerializeField] private bool _pause = true;

        [Space]

        [SerializeField] private Vector3 _noiseShift = Vector3.zero;
        [SerializeField] private Vector3 _warpShift = Vector3.zero;

        [Space]

        [SerializeField, Range(1, 16)]
        private int _downScaler = 16;

        [Space]

        [SerializeField]
        private Gradient _colorPalette;

        [SerializeField]
        private RawImage _preview;

        [SerializeField]
        private Camera _camera;
        private int _width, _height;

        private float[] _noiseMap;
        private Color[] _colorMap;

        private Texture2D _texture;

        private void Update()
        {
            if (_pause)
                return;

            _width = _camera.pixelWidth / _downScaler;
            _height = _camera.pixelHeight / _downScaler;

            _noiseMap = new float[_width * _height];

            if (_noiseProfile.Warp)
            {
                _noiseProfile.Init();
                _warpProfile.Init();
            }
            else
            {
                _noiseProfile.Init();
            }

            if (_useGPU)
            {
                ComputeBuffer noiseBuffer = new ComputeBuffer(_noiseMap.Length, sizeof(float));
                noiseBuffer.SetData(_noiseMap);

                FastNoise2D.Generate(ref noiseBuffer, _width, _height, _toggle3D, _noiseProfile, _warpProfile);

                noiseBuffer.GetData(_noiseMap);
                noiseBuffer.Release();
            }
            else
            {
                FastNoise2D.Generate(ref _noiseMap, _width, _height, _toggle3D, _noiseProfile, _warpProfile);
            }

            _colorMap = new Color[_width * _height];
            for (int i = 0; i < _colorMap.Length; i++)
            {
                _colorMap[i] = _colorPalette.Evaluate(_noiseMap[i]);
            }

            _texture = new Texture2D(_width, _height)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Point
            };
            _texture.SetPixels(_colorMap);
            _texture.Apply();

            _preview.texture = _texture;

            _noiseProfile.AnimateOffset(_noiseShift);
            _warpProfile.AnimateOffset(_warpShift);
        }
    }
}