using UnityEngine;
using UnityEngine.UI;

namespace Utils.Colors.Coloring
{
    public sealed class GradientColoringDemo : MonoBehaviour
    {
        [SerializeField]
        private bool _useGPU = false;
        [SerializeField]
        private bool _enableColoring = false;

        [Space]

        [SerializeField]
        private Texture2D _texture;

        [Space]

        [SerializeField]
        private Gradient _gradient;

        [Space]

        [SerializeField]
        private RawImage _image;

        private void Update()
        {
            Color[] colors = new Color[_texture.width * _texture.height];

            Texture2D texture = new Texture2D(_texture.width, _texture.height);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;

            if (_enableColoring)
            {
                float[] values = new float[colors.Length];
                for (int y = 0, i = 0; y < _texture.height; y++)
                {
                    for (int x = 0; x < _texture.width; x++, i++)
                    {
                        Color pixel = _texture.GetPixel(x, y);
                        values[i] = (pixel.r + pixel.g + pixel.b) / 3.0f;
                    }
                }

                if (_useGPU)
                {
                    ComputeBuffer colorsBuffer = new ComputeBuffer(colors.Length, sizeof(float) * 4);
                    colorsBuffer.SetData(colors);

                    ComputeBuffer valuesBuffer = new ComputeBuffer(values.Length, sizeof(float));
                    valuesBuffer.SetData(values);

                    Coloring.GradientColoringGPU(ref colorsBuffer, valuesBuffer, _gradient);

                    colorsBuffer.GetData(colors);
                    colorsBuffer.Release();
                }
                else
                {
                    Coloring.GradientColoringCPU(ref colors, values, _gradient);
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
