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
            float[] curve = new float[NumOfPoints + 1];
            for (int i = 0; i < curve.Length; i++)
            {
                curve[i] = (float)i / NumOfPoints;
            }

            Curves.SetToCurve(
                ref curve, 
                (Curves.Equations)_curveDropdown.value, 
                _powerSlider.value
            );

            for (int i = 0; i < curve.Length; i++)
            {
                float xx = Mathf.Lerp(MinY, MaxY, (float)i / NumOfPoints);

                float v = 0.0f;
                if (_flipToggle.isOn)
                {
                    v = Mathf.Lerp(MinY, MaxY, Mathf.Clamp01(curve[curve.Length - 1 - i]) * _strengthSlider.value);
                }
                else
                {
                    v = Mathf.Lerp(MinY, MaxY, Mathf.Clamp01(curve[i]) * _strengthSlider.value);
                }

                if (_invertToggle.isOn)
                {
                    _lineRenderer.SetPosition(i, new Vector3(xx, 1 - v, -1));
                }
                else
                {
                    _lineRenderer.SetPosition(i, new Vector3(xx, v, -1));
                }
            }
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