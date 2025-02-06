using Utils.Noise;
using Utils.Curves;
using Utils.Colors.Blend;
using Utils.Colors.Coloring;
using Utils.Filters.Blur;
using UnityEngine;

namespace Utils.Init
{
    public class UtilsInit : MonoBehaviour
    {
        [Header("Shaders")]

        [SerializeField] private ComputeShader _fastNoise2D;

        [Space]

        [SerializeField] private ComputeShader _curves;
        [SerializeField] private ComputeShader _biasAndGain;
        [SerializeField] private ComputeShader _animationCurve;

        [Space]
        [Header("Shaders/Filters")]
        [SerializeField] private ComputeShader _box;

        [Header("Shaders/Colors")]

        [SerializeField] private ComputeShader _coloring;
        [SerializeField] private ComputeShader _colorMixer;

        private void Awake()
        {
            FastNoise2DGPU.Shader = _fastNoise2D;

            CurvesGPU.Shader = _curves;
            BiasAndGainGPU.Shader = _biasAndGain;
            AnimationCurves.Shader = _animationCurve;

            BoxBlur.Shader = _box;

            Coloring.Shader = _coloring;
            MixGPU.Shader = _colorMixer;
        }
    }
}
