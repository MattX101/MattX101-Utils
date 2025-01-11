using UnityEngine;
using UnityEngine.UI;

namespace Utils.Colors.Coloring
{
    public class GradientColoringDemo : MonoBehaviour
    {
        [SerializeField]
        private ComputeShader _shader;

        [Space]

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

        private void Start()
        {
            Coloring.Shader = _shader;
        }

        private void Update()
        {
            Color[] colors = new Color[_texture.width * _texture.height];

            Texture2D texture = new Texture2D(_texture.width, _texture.height);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;

            if (_enableColoring)
            {
                float[] noise = new float[colors.Length];
                for (int y = 0, i = 0; y < _texture.height; y++)
                {
                    for (int x = 0; x < _texture.width; x++, i++)
                    {
                        noise[i] = _texture.GetPixel(x, y).r;
                    }
                }

                if (_useGPU)
                {
                    ComputeBuffer colorsBuffer = new ComputeBuffer(colors.Length, sizeof(float) * 4);
                    colorsBuffer.SetData(colors);

                    ComputeBuffer valuesBuffer = new ComputeBuffer(noise.Length, sizeof(float));
                    valuesBuffer.SetData(noise);

                    Coloring.GradientColoringGPU(ref colors, colorsBuffer, valuesBuffer, _gradient);
                }
                else
                {
                    Coloring.GradientColoringCPU(ref colors, noise, _gradient);
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
