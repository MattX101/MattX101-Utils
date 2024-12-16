using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace Utils.Curves
{
    public sealed class CurveDemo : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer _lineRenderer;

        private const int NumOfPoints = 100;

        private const float MinX = -130.0f;
        private const float MaxX = 130.0f;
        private const float MinY = -130.0f;
        private const float MaxY = 130.0f;

        [Header("Toggles")]
        [SerializeField] private Toggle _invertToggle;
        [SerializeField] private Toggle _flipToggle;

        [Header("Sliders")]
        [SerializeField] private Slider _strengthSlider;
        [SerializeField] private Slider _powerSlider;

        [Space]

        [SerializeField] private TMP_Text _strengthSliderText;
        [SerializeField] private TMP_Text _powerSliderText;

        [Header("Dropdown")]
        [SerializeField] private TMP_Dropdown _curveDropdown;
        private enum Equations
        {
            EaseIn,
            EaseInCirc,
            EaseInOut,
            EaseInOiutSine,
            Sine,
            SineSqrt,
            RepeatedSine,
            HalfDownUpSine,
            SineFrequency,
            RadianArcSine,
            RadianArcSineSqrt
        };

        private delegate float Equation(float v, float p);

        private void Awake()
        {
            _powerSliderText.text = "2";
            _strengthSliderText.text = "1";
            _lineRenderer.positionCount = NumOfPoints + 1;
        }

        private void Start()
        {
            Compute();
        }

        public void Compute()
        {
            Equation equation = GetEquation(_curveDropdown.value);

            if (!_invertToggle.isOn && !_flipToggle.isOn)
            {
                for (int x = 0; x < NumOfPoints + 1; x++)
                {
                    float i = (float)x / NumOfPoints;

                    float xx = Mathf.Lerp(MinY, MaxY, i);
                    float v = Mathf.Lerp(MinY, MaxY, Mathf.Clamp01(equation(i, _powerSlider.value)) * _strengthSlider.value);

                    _lineRenderer.SetPosition(x, new Vector3(xx, v, -1));
                }
            }
            else if (_invertToggle.isOn && !_flipToggle.isOn)
            {
                for (int x = 0; x < NumOfPoints + 1; x++)
                {
                    float i = (float)x / NumOfPoints;

                    float xx = Mathf.Lerp(MinY, MaxY, i);
                    float v = Mathf.Lerp(MinY, MaxY, Mathf.Clamp01(equation(i, _powerSlider.value)) * _strengthSlider.value);

                    _lineRenderer.SetPosition(x, new Vector3(xx, 1 - v, -1));
                }
            }
            else if (!_invertToggle.isOn && _flipToggle.isOn)
            {
                for (int x = 0; x < NumOfPoints + 1; x++)
                {
                    float i = (float)x / NumOfPoints;

                    float xx = Mathf.Lerp(MinY, MaxY, i);
                    float v = Mathf.Lerp(MinY, MaxY, Mathf.Clamp01(equation(1 - i, _powerSlider.value)) * _strengthSlider.value);

                    _lineRenderer.SetPosition(x, new Vector3(xx, v, -1));
                }
            }
            else
            {
                for (int x = 0; x < NumOfPoints + 1; x++)
                {
                    float i = (float)x / NumOfPoints;

                    float xx = Mathf.Lerp(MinY, MaxY, i);
                    float v = Mathf.Lerp(MinY, MaxY, Mathf.Clamp01(equation(1 - i, _powerSlider.value)) * _strengthSlider.value);

                    _lineRenderer.SetPosition(x, new Vector3(xx, 1 - v, -1));
                }
            }
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
                7 => Curves.HalfDownUpSine,
                8 => Curves.SineFrequency,
                9 => Curves.RadianArcSine,
                10 => Curves.RadianArcSineSqrt,
                _ => Curves.EaseIn
            };
        }

        public void OnStrengthSliderValueChange()
        {
            _strengthSliderText.text = Math.Round(_strengthSlider.value, 2).ToString();
        }

        public void OnPowerSliderValueChange()
        {
            _powerSliderText.text = Math.Round(_powerSlider.value, 2).ToString();
        }
    }
}