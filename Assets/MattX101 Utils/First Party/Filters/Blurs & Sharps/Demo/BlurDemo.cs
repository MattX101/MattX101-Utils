using Utils.Filters.Sharpen;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.Filters.Blur
{
    public class BlurDemo : MonoBehaviour
    {
        [SerializeField] private Texture2D _texture;
        [SerializeField] private RawImage _image;

        private Color[] _source, _filter;
        private Texture2D _previewTexture;

        private enum Mode
        {
            None,
            Blur,
            Sharpen
        };
        [SerializeField]
        private Mode _mode;

        [SerializeField] private bool _useGPU = true;

        private void Awake()
        {
            _previewTexture = new Texture2D(_texture.width, _texture.height);
            _previewTexture.wrapMode = TextureWrapMode.Clamp;
            _previewTexture.filterMode = FilterMode.Point;

            _filter = new Color[_texture.width * _texture.height];
            _source = new Color[_texture.width * _texture.height];
            _source = _texture.GetPixels();
        }

        private void Update()
        {
            if (_mode == Mode.Blur)
            {
                if (_useGPU)
                {
                    ComputeBuffer filterBuffer = new ComputeBuffer(_filter.Length, sizeof(float) * 4);
                    filterBuffer.SetData(_filter);

                    ComputeBuffer sourceBuffer = new ComputeBuffer(_source.Length, sizeof(float) * 4);
                    sourceBuffer.SetData(_source);

                    BoxBlur.Compute(ref filterBuffer, sourceBuffer, _texture.width, _texture.height, false);
                    filterBuffer.GetData(_filter);

                    _previewTexture.SetPixels(_filter);

                    filterBuffer.Dispose();
                    sourceBuffer.Dispose();
                }
                else
                {
                    BoxBlur.Compute(ref _filter, _source, _texture.width, _texture.height);
                    _previewTexture.SetPixels(_filter);
                }
            }
            else if (_mode == Mode.Sharpen)
            {
                if (_useGPU)
                {
                    ComputeBuffer filterBuffer = new ComputeBuffer(_filter.Length, sizeof(float) * 4);
                    filterBuffer.SetData(_filter);

                    ComputeBuffer sourceBuffer = new ComputeBuffer(_source.Length, sizeof(float) * 4);
                    sourceBuffer.SetData(_source);

                    BoxSharpen.Compute(ref filterBuffer, sourceBuffer, _texture.width, _texture.height, false);
                    filterBuffer.GetData(_filter);

                    _previewTexture.SetPixels(_filter);

                    filterBuffer.Dispose();
                    sourceBuffer.Dispose();
                }
                else
                {
                    BoxSharpen.Compute(ref _filter, _source, _texture.width, _texture.height);
                    _previewTexture.SetPixels(_filter);
                }
            }
            else
            {
                _previewTexture.SetPixels(_source);
            }

            _previewTexture.Apply();

            _image.texture = _previewTexture;
        }
    }
}
