using UnityEngine;
using UnityEngine.UI;

namespace Utils.Colors.Coloring
{
    public sealed class ColoringDemo : MonoBehaviour
    {
        [SerializeField]
        private bool _useGPU;
        [SerializeField]
        private bool _enableColoring;

        [Space]

        [SerializeField]
        private Texture2D _texture;

        [Space]

        [SerializeField]
        private Color _color;

        [Space]

        [SerializeField]
        private RawImage _image;

        private void Update()
        {
            Color[] colors = new Color[_texture.width * _texture.height];

            Texture2D texture = new Texture2D(_texture.width, _texture.height)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Point
            };

            if (_enableColoring)
            {
                float[] values = new float[colors.Length];
                for (int y = 0, i = 0; y < _texture.height; y++)
                {
                    for (int x = 0; x < _texture.width; x++, i++)
                    {
                        values[i] = _texture.GetPixel(x, y).r;
                    }
                }

                if (_useGPU)
                {
                    ComputeBuffer colorsBuffer = new ComputeBuffer(colors.Length, sizeof(float) * 4);
                    colorsBuffer.SetData(colors);

                    ComputeBuffer valuesBuffer = new ComputeBuffer(values.Length, sizeof(float));
                    valuesBuffer.SetData(values);

                    Coloring.Color(ref colorsBuffer, valuesBuffer, _color);

                    colorsBuffer.GetData(colors);
                    colorsBuffer.Release();
                }
                else
                {
                    Coloring.Color(ref colors, values, _color);
                }
            }
            else
            {
                colors = _texture.GetPixels();
            }

            texture.SetPixels(colors);
            texture.Apply();

            _image.texture = texture;
        }
    }
}
