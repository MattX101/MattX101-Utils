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

        [SerializeField]
        private MaskSettings _maskSettings;

        private float[] _mask;
        private Color[] _colors;

        private Texture2D _texture;

        void Update()
        {
            _mask = new float[_camera.pixelWidth * _camera.pixelHeight];
            _colors = new Color[_camera.pixelWidth * _camera.pixelHeight];

            MaskGenerator.Generate(ref _mask, _camera.pixelWidth, _camera.pixelHeight, _maskSettings);

            for (int i = 0; i < _colors.Length; i++)
            {
                _colors[i] = Color.white * _mask[i];
                _colors[i].a = 1;
            }

            _texture = new Texture2D(_camera.pixelWidth, _camera.pixelHeight);
            _texture.wrapMode = TextureWrapMode.Clamp;
            _texture.filterMode = FilterMode.Point;
            _texture.SetPixels(_colors);
            _texture.Apply();

            _image.texture = _texture;
        }
    }
}
