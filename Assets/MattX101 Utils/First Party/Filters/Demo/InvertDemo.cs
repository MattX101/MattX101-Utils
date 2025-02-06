using UnityEngine;
using UnityEngine.UI;

namespace Utils.Filters
{
    public class InvertDemo : MonoBehaviour
    {
        [SerializeField] private Texture2D _texture;
        [SerializeField] private RawImage _image;

        [SerializeField] private bool _useGPU = true;
        [SerializeField] private bool _modify = true;

        private Color[] _source, _filter;
        private Texture2D _previewTexture;

        void Start()
        {
            _previewTexture = new Texture2D(_texture.width, _texture.height);
            _previewTexture.wrapMode = TextureWrapMode.Clamp;
            _previewTexture.filterMode = FilterMode.Point;

            _filter = new Color[_texture.width * _texture.height];
            _source = new Color[_texture.width * _texture.height];
            _source = _texture.GetPixels();
        }

        void Update()
        {
            if (_modify)
            {
                _source.CopyTo(_filter, 0);

                if (_useGPU)
                {
                    ComputeBuffer buffer = new ComputeBuffer(_filter.Length, sizeof(float) * 4);
                    buffer.SetData(_filter);

                    Invert.Compute(ref buffer, false);
                    buffer.GetData(_filter);
                    buffer.Release();
                }
                else
                {
                    Invert.Compute(ref _filter);
                }

                _previewTexture.SetPixels(_filter);
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
