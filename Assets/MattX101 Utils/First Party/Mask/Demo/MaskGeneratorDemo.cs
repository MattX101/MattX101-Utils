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

        private Color[] _colors;

        private Texture2D _texture;

        void Update()
        {
            _colors = new Color[_camera.pixelWidth * _camera.pixelHeight];

            MaskGenerator.Generate(ref _colors, _camera.pixelWidth, _camera.pixelHeight, _maskSettings);

            _texture = new Texture2D(_camera.pixelWidth, _camera.pixelHeight);
            _texture.wrapMode = TextureWrapMode.Clamp;
            _texture.filterMode = FilterMode.Point;
            _texture.SetPixels(_colors);
            _texture.Apply();

            _image.texture = _texture;
        }
    }
}
