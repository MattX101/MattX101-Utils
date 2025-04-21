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

        private Texture2D _previewTexture;

        private ComputeBuffer _colorsBuffer;

        private delegate float Equation(float v, float p);

        private void Awake()
        {
            _colorsBuffer = new ComputeBuffer(_texture.width * _texture.height, sizeof(float) * 4);

            _previewTexture = new Texture2D(_texture.width, _texture.height)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Point
            };
        }

        private void Start()
        {
            _previewTexture.SetPixels(_texture.GetPixels());
            _previewTexture.Apply();

            _image.texture = _previewTexture;

            SetButtomColor(_buttonImage, _modify);
            SetButtomColor(_useGPUImage, _useGPU);

            Calculate();
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

        public void Calculate()
        {
            Color[] colors = _texture.GetPixels();

            _powerText.text = Math.Round(_powerSlider.value, 2).ToString();

            if (_modify)
            {
                if (_useGPU)
                {
                    _colorsBuffer = new ComputeBuffer(colors.Length, sizeof(float) * 4);
                    _colorsBuffer.SetData(colors);

                    Curves.SetToCurve(ref _colorsBuffer, (Curves.Equations)_dropdown.value, _powerSlider.value, false);
                    _colorsBuffer.GetData(colors);
                }
                else
                {
                    Curves.SetToCurve(
                        ref colors, 
                        (Curves.Equations)_dropdown.value,
                        _powerSlider.value);
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
