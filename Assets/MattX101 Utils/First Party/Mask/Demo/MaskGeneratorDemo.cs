using UnityEngine;
using UnityEngine.UI;

namespace Utils.Mask
{
    internal class MaskGeneratorDemo : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private RawImage _image;

        [Space]

        [SerializeField, Range(1, 16)]
        private int _downScaler = 1;

        [Space]

        [SerializeField]
        private bool _useGPU = false;

        [Space]

        [SerializeField]
        private MaskSettings _maskSettings;

        private float[] _mask;
        private Color[] _colors;

        private Texture2D _texture;

        void Update()
        {
            int width = _camera.pixelWidth / _downScaler;
            int height = _camera.pixelHeight / _downScaler;

            _mask = new float[width * height];
            _colors = new Color[width * height];

            if (_useGPU)
            {
                ComputeBuffer buffer = new ComputeBuffer(width * height, sizeof(float));
                buffer.SetData(_mask);

                MaskGenerator.Generate(ref buffer, width, height, _maskSettings);

                buffer.GetData(_mask);
                buffer.Dispose();
            }
            else
            {
                MaskGenerator.Generate(ref _mask, width, height, _maskSettings);
            }

            for (int i = 0; i < _colors.Length; i++)
            {
                _colors[i] = Color.white * _mask[i];
                _colors[i].a = 1;
            }

            _texture = new Texture2D(width, height);
            _texture.wrapMode = TextureWrapMode.Clamp;
            _texture.filterMode = FilterMode.Point;
            _texture.SetPixels(_colors);
            _texture.Apply();

            _image.texture = _texture;
        }
    }
}
