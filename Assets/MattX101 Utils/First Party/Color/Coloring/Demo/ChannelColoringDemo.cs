using UnityEngine;
using UnityEngine.UI;

namespace Utils.Colors.Coloring
{
    public class ChannelColoringDemo : MonoBehaviour
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
        private Coloring.Channel _channel;

        [SerializeField]
        private Color _color;

        [Space]

        [SerializeField]
        private RawImage _image;

        private void Start()
        {
            Coloring.Shader = _shader;
        }

        private void Update()
        {
            Color[] colors = _texture.GetPixels();

            Texture2D texture = new Texture2D(_texture.width, _texture.height);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;

            if (_enableColoring)
            {
                if (_useGPU)
                {
                    ComputeBuffer colorsBuffer = new ComputeBuffer(colors.Length, sizeof(float) * 4);
                    colorsBuffer.SetData(colors);

                    Coloring.ChannelColorigGPU(ref colors, colorsBuffer, _channel, _color);
                }
                else
                {
                    Coloring.ChannelColorigCPU(ref colors, _channel, _color);
                }
            }

            texture.SetPixels(colors);
            texture.Apply();

            _image.texture = texture;
        }
    }
}
