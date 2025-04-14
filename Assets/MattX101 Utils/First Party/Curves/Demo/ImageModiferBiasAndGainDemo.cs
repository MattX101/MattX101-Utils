using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace Utils.Curves
{
    public class ImageModiferBiasAndGainDemo : MonoBehaviour
    {
        [SerializeField] private Texture2D _texture;
        [SerializeField] private RawImage _image;

        [Space]
        [Header("UI Elements")]
        [Space]

        [SerializeField] private Slider _biasSlider;
        [SerializeField] private TMP_Text _biasText;

        [Space]

        [SerializeField] private Slider _gainSlider;
        [SerializeField] private TMP_Text _gainText;

        [Space]

        [SerializeField] private Image _buttonImage;
        [SerializeField] private Image _useGPUImage;

        private bool _useGPU = true;
        private bool _modify = true;

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

            SetButtomColor(_buttonImage, _modify);
            SetButtomColor(_useGPUImage, _useGPU);
        }

        public void Modify()
        {
            _modify = !_modify;
            SetButtomColor(_buttonImage, _modify);
        }

        public void UseGPU()
        {
            _useGPU = !_useGPU;
            SetButtomColor(_useGPUImage, _useGPU);
        }

        private void SetButtomColor(Image image, bool toggle)
        {
            image.color = toggle ? Color.green : Color.red;
        }

        public void Calcualte()
        {
            Color[] colors = _texture.GetPixels();

            _biasText.text = Math.Round(_biasSlider.value, 2).ToString();
            _gainText.text = Math.Round(_gainSlider.value, 2).ToString();

            if (_modify)
            {
                if (_useGPU)
                {
                    _colorsBuffer = new ComputeBuffer(colors.Length, sizeof(float) * 4);
                    _colorsBuffer.SetData(colors);

                    BiasAndGain.ModifyImage(ref _colorsBuffer, _biasSlider.value, _gainSlider.value);
                    _colorsBuffer.GetData(colors);
                }
                else
                {
                    for (int i = 0; i < colors.Length; i++)
                    {
                        colors[i].r = BiasAndGain.Gain(BiasAndGain.Bias(colors[i].r, _biasSlider.value), _gainSlider.value);
                        colors[i].g = BiasAndGain.Gain(BiasAndGain.Bias(colors[i].g, _biasSlider.value), _gainSlider.value);
                        colors[i].b = BiasAndGain.Gain(BiasAndGain.Bias(colors[i].b, _biasSlider.value), _gainSlider.value);
                    }
                }
            }

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
