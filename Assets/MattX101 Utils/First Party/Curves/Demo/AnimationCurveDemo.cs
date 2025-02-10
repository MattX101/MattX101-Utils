using UnityEngine;
using UnityEngine.UI;

namespace Utils.Curves
{
    public class AnimationCurveDemo : MonoBehaviour
    {
        [SerializeField] private UnityEngine.AnimationCurve _curve;
        
        [SerializeField] private Texture2D _texture;
        [SerializeField] private RawImage _image;

        private Color[] _textureColors;
        private Texture2D _previewTexture;

        private ComputeBuffer _colorsBuffer;

        private void Awake()
        {
            _colorsBuffer = new ComputeBuffer(_texture.width * _texture.height, sizeof(float) * 4);

            _previewTexture = new Texture2D(_texture.width, _texture.height);
            _previewTexture.wrapMode = TextureWrapMode.Clamp;
            _previewTexture.filterMode = FilterMode.Point;
        }

        private void Start()
        {
            _previewTexture.SetPixels(_texture.GetPixels());
            _previewTexture.Apply();

            _image.texture = _previewTexture;

            Calcualte();
        }

        public void Calcualte()
        {
            Color[] colors = _texture.GetPixels();

            _colorsBuffer = new ComputeBuffer(colors.Length, sizeof(float) * 4);
            _colorsBuffer.SetData(colors);

            Curves.SetToCurve(_colorsBuffer, _curve, false);
            _colorsBuffer.GetData(colors);

            _previewTexture.SetPixels(colors);
            _previewTexture.Apply();

            _image.texture = _previewTexture;
        }

        private void OnDestroy()
        {
            _colorsBuffer.Release();
        }
    }
}
