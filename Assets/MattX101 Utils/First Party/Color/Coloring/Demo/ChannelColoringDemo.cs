using UnityEngine;
using UnityEngine.UI;

namespace Utils.Colors.Coloring
{
    public sealed class ChannelColoringDemo : MonoBehaviour
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
        private Coloring.Channel _channel;

        [SerializeField]
        private Color _color;

        [Space]

        [SerializeField]
        private RawImage _image;

        private void Update()
        {
            Color[] colors = _texture.GetPixels();

            Texture2D texture = new Texture2D(_texture.width, _texture.height)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Point
            };

            if (_enableColoring)
            {
                if (_useGPU)
                {
                    ComputeBuffer colorsBuffer = new ComputeBuffer(colors.Length, sizeof(float) * 4);
                    colorsBuffer.SetData(colors);

                    Coloring.ChannelColoring(ref colorsBuffer, _channel, _color);

                    colorsBuffer.GetData(colors);
                    colorsBuffer.Release();
                }
                else
                {
                    Coloring.ChannelColoring(ref colors, _channel, _color);
                }
            }

            texture.SetPixels(colors);
            texture.Apply();

            _image.texture = texture;
        }
    }
}
