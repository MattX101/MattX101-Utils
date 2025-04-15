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

        [SerializeField]
        private bool _useGPU = false, _toggle3D = true, _puase = true;

        [Space]

        [SerializeField] private Vector3 _noiseShift = Vector3.zero;
        [SerializeField] private Vector3 _warpShift = Vector3.zero;

        [SerializeField, Range(1, 16)]
        private int _downScaler = 16;

        [Space]

        [SerializeField]
        private Gradient _colorPalette;

        [SerializeField]
        private RawImage _preview;

        private Camera _camera;
        private int _cameraWidth, _cameraHeight;

        private float[] _noiseMap;
        private Color[] _colorMap;

        private Texture2D _texture;

        private void Awake()
        {
            _camera = FindObjectOfType<Camera>();
        }

        private void Update()
        {
            if (_puase)
                return;

            _cameraWidth = _camera.pixelWidth / _downScaler;
            _cameraHeight = _camera.pixelHeight / _downScaler;

            _noiseMap = new float[_cameraWidth * _cameraHeight];

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

                FastNoise2D.Generate(ref noiseBuffer, _cameraWidth, _cameraHeight, _toggle3D, _noiseProfile, _warpProfile);

                noiseBuffer.GetData(_noiseMap);
                noiseBuffer.Release();
            }
            else
            {
                FastNoise2D.Generate(ref _noiseMap, _cameraWidth, _cameraHeight, _toggle3D, _noiseProfile, _warpProfile);
            }

            _colorMap = new Color[_cameraWidth * _cameraHeight];
            for (int i = 0; i < _colorMap.Length; i++)
            {
                _colorMap[i] = _colorPalette.Evaluate(_noiseMap[i]);
            }

            _texture = new Texture2D(_cameraWidth, _cameraHeight)
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