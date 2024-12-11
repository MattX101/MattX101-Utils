# if UNITY_EDITOR
using Utils.Colors.Model;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace Utils.Colors
{
    public class ColorConversionDemo : MonoBehaviour
    {
        [Header("Dropdowns")]
        [SerializeField] private TMP_Dropdown _currentModelDropdown;
        [SerializeField] private TMP_Dropdown _conversionModelDropdown;

        [Header("Text")]
        [SerializeField] private TMP_Text _sliderAText;
        [SerializeField] private TMP_Text _sliderBText;
        [SerializeField] private TMP_Text _sliderCText;

        [Header("Sliders")]
        [SerializeField] private Slider _sliderASlider;
        [SerializeField] private Slider _sliderBSlider;
        [SerializeField] private Slider _sliderCSlider;

        [Header("Values Text")]
        [SerializeField] private TMP_Text _sliderAValueText;
        [SerializeField] private TMP_Text _sliderBValueText;
        [SerializeField] private TMP_Text _sliderCValueText;

        [Header("Preview")]
        [SerializeField] private RawImage _preview;
        [SerializeField] private TMP_Text _hex;
        [SerializeField] private TMP_Text _rgb;

        [Header("Text")]
        [SerializeField] private TMP_Text _conversionAText;
        [SerializeField] private TMP_Text _conversionBText;
        [SerializeField] private TMP_Text _conversionCText;

        [Header("Values Text")]
        [SerializeField] private TMP_Text _conversionAValueText;
        [SerializeField] private TMP_Text _conversionBValueText;
        [SerializeField] private TMP_Text _conversionCValueText;

        [SerializeField] private TMP_Text _isValidText;

        private float _sliderAValue, _sliderBValue, _sliderCValue;

        private enum Models
        {
            RGB, HSL, HSV, HEX
        }

        private void Start()
        {
            OnDropdownChange();

            Vector3 values = Vector3.zero;
            Color filler = Color.black;
            SetValues(ref filler, ref values, _conversionAText, _conversionBText, _conversionCText, _conversionModelDropdown.value);

            OnSliderAValueChange();
            OnSliderBValueChange();
            OnSliderCValueChange();
        }

        public void OnDropdownChange()
        {                
            Vector3 values = Vector3.zero;
            Color filler = Color.black;
            SetValues(ref filler, ref values, _sliderAText, _sliderBText, _sliderCText, _currentModelDropdown.value);

            _sliderASlider.value = values.x;
            _sliderBSlider.value = values.y;
            _sliderCSlider.value = values.z;

            _sliderAValue = values.x;
            _sliderBValue = values.y;
            _sliderCValue = values.z;

            Convert();
        }

        public void OnSliderAValueChange() => _sliderAValue = _sliderASlider.value;
        public void OnSliderBValueChange() => _sliderBValue = _sliderBSlider.value;
        public void OnSliderCValueChange() => _sliderCValue = _sliderCSlider.value;

        public void Convert()
        {
            float a  = _currentModelDropdown.value == 0 ? 255 : 360;
            float bc = _currentModelDropdown.value == 0 ? 255 : 100;

            _sliderAValueText.text = Math.Round(_sliderAValue * a,  2).ToString();
            _sliderBValueText.text = Math.Round(_sliderBValue * bc, 2).ToString();
            _sliderCValueText.text = Math.Round(_sliderCValue * bc, 2).ToString();

            Color color = Color.black;
            switch (_currentModelDropdown.value)
            {
                case 0: color = new Color(_sliderAValue, _sliderBValue, _sliderCValue, 1); break;
                case 1: color = ColorConversion.HSLToRGB(new HSL(_sliderAValue * 360.0f, _sliderBValue, _sliderCValue)); break;
                case 2: color = ColorConversion.HSVToRGB(new HSV(_sliderAValue * 360.0f, _sliderBValue, _sliderCValue)); break;
            }

            _preview.color = color;

            HEX hex = ColorConversion.RGBToHex(_preview.color);
            _hex.text = "#" + hex.Hex;

            Color rgb = ColorConversion.HEXToRGB(hex);
            _rgb.text = ((int)(rgb.r * 255)).ToString() + " - " + ((int)(rgb.g * 255)).ToString() + " - " + ((int)(rgb.b * 255)).ToString();

            Vector3 values = Vector3.zero;
            Color reverseConversion = Color.black;
            SetValues(ref reverseConversion, ref values, _conversionAText, _conversionBText, _conversionCText, _conversionModelDropdown.value);

            a  = _conversionModelDropdown.value == 0 ? 255 : 360;
            bc = _conversionModelDropdown.value == 0 ? 255 : 100;

            _conversionAValueText.text = Math.Round(values.x * a,  2).ToString();
            _conversionBValueText.text = Math.Round(values.y * bc, 2).ToString();
            _conversionCValueText.text = Math.Round(values.z * bc, 2).ToString();

            _isValidText.text = _preview.color == reverseConversion ? "True" : "False";
        }

        private void SetValues(ref Color color, ref Vector3 values, TMP_Text a, TMP_Text b, TMP_Text c, int dropdownValue)
        {
            if (dropdownValue == (int)Models.RGB)
            {
                color = _preview.color;

                values.x = _preview.color.r;
                values.y = _preview.color.g;
                values.z = _preview.color.b;

                a.text = "Red";
                b.text = "Green";
                c.text = "Blue";
            }
            else if (dropdownValue == (int)Models.HSL)
            {
                HSL hsl = ColorConversion.RGBToHSL(_preview.color);
                color = ColorConversion.HSLToRGB(hsl);

                values.x = hsl.Hue / 360.0f;
                values.y = hsl.Saturation;
                values.z = hsl.Lightness;

                a.text = "Hue";
                b.text = "Saturation";
                c.text = "Lightness";
            }
            else if (dropdownValue == (int)Models.HSV)
            {
                HSV hsv = ColorConversion.RGBToHSV(_preview.color);
                color = ColorConversion.HSVToRGB(hsv);

                values.x = hsv.Hue / 360.0f;
                values.y = hsv.Saturation;
                values.z = hsv.Value;

                a.text = "Hue";
                b.text = "Saturation";
                c.text = "Value";
            }
        }
    }
}
#endif