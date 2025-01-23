using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace Utils.Curves
{
    public class ImageModiferCurvesDemo : MonoBehaviour
    {
        [SerializeField] private Texture2D _texture;
        [SerializeField] private RawImage _image;

        [Space]
        [Header("UI Elements")]
        [Space]

        [SerializeField] private Slider _powerSlider;
        [SerializeField] private TMP_Text _powerText;

        [Space]
        [SerializeField] private TMP_Dropdown _dropdown;

        [Space]

        [SerializeField] private Image _buttonImage;
        [SerializeField] private Image _useGPUImage;

        private bool _useGPU = true;
        private bool _modify = true;

        private Color[] _textureColors;
        private Texture2D _previewTexture;

        private ComputeBuffer _colorsBuffer;

        private delegate float Equation(float v, float p);

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

            Calcualte();
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

            _powerText.text = Math.Round(_powerSlider.value, 2).ToString();

            if (_modify)
            {
                if (_useGPU)
                {
                    _colorsBuffer = new ComputeBuffer(colors.Length, sizeof(float) * 4);
                    _colorsBuffer.SetData(colors);

                    CurvesGPU.ModifyImage(ref _colorsBuffer, _dropdown.value, _powerSlider.value);
                    _colorsBuffer.GetData(colors);
                }
                else
                {
                    Equation equation = GetEquation(_dropdown.value);
                    for (int i = 0; i < colors.Length; i++)
                    {
                        colors[i].r = equation(colors[i].r, _powerSlider.value);
                        colors[i].g = equation(colors[i].g, _powerSlider.value);
                        colors[i].b = equation(colors[i].b, _powerSlider.value);
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

        private Equation GetEquation(int index)
        {
            return index switch
            {
                0 => Curves.EaseIn,
                1 => Curves.EaseInCirc,
                2 => Curves.EaseInOut,
                3 => Curves.EaseInOutSine,

                4 => Curves.Sine,
                5 => Curves.SineSqrt,
                6 => Curves.RepeatedSine,
                7 => Curves.SineFrequency,
                8 => Curves.RadianArcSine,
                9 => Curves.RadianArcSineSqrt,
                10 => Curves.HalfDownUpSine,

                _ => Curves.EaseIn
            };
        }
    }
}
