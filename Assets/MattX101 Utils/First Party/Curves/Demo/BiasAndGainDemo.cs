using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Utils.Curves
{
    public class BiasAndGainDemo : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer _lineRenderer;

        private const int NumOfPoints = 100;

        private const float MinY = -130.0f;
        private const float MaxY = 130.0f;

        [Header("Sliders")]
        [SerializeField] private Slider _biasSlider;
        [SerializeField] private Slider _gainSlider;

        [Space]

        [SerializeField] private TMP_Text _biasSliderText;
        [SerializeField] private TMP_Text _gainSliderText;

        private void Awake()
        {
            _lineRenderer.positionCount = NumOfPoints + 1;
        }

        private void Start()
        {
            Compute();
        }

        public void Compute()
        {
            for (int x = 0; x < NumOfPoints + 1; x++)
            {
                float i = (float)x / NumOfPoints;

                float xx = Mathf.Lerp(MinY, MaxY, i);
                float v = BiasAndGain.Bias(i, _biasSlider.value);
                v = BiasAndGain.Gain(v, _gainSlider.value);
                v = Mathf.Lerp(MinY, MaxY, v);

                _lineRenderer.SetPosition(x, new Vector3(xx, v, -1));
            }
        }

        public void OnBiasSliderValueChange()
        {
            _biasSliderText.text = Math.Round(_biasSlider.value, 2).ToString();
        }

        public void OnGainSliderValueChange()
        {
            _gainSliderText.text = Math.Round(_gainSlider.value, 2).ToString();
        }
    }
}
