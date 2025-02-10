using Utils.Noise;
using Utils.Curves;
using Utils.Colors.Blend;
using Utils.Colors.Coloring;
using Utils.Filters.Blur;
using UnityEngine;
using Utils.Filters;

namespace Utils.Init
{
    public class UtilsInit : MonoBehaviour
    {
        [Header("Shaders")]

        [SerializeField] private ComputeShader _fastNoise2D;

        [Space]
        [Header("Shaders/Curves")]

        [SerializeField] private ComputeShader _curves;
        [SerializeField] private ComputeShader _animationCurve;
        [SerializeField] private ComputeShader _biasAndGain;

        [Space]
        [Header("Shaders/Filters")]

        [SerializeField] private ComputeShader _box;
        [SerializeField] private ComputeShader _invert;

        [Header("Shaders/Colors")]

        [SerializeField] private ComputeShader _coloring;
        [SerializeField] private ComputeShader _colorMixer;

        private void Awake()
        {
            FastNoise2D.Shader = _fastNoise2D;

            Curves.Curves.Shader = _curves;
            Curves.Curves.AnimationCurveShader = _animationCurve;
            BiasAndGain.Shader = _biasAndGain;

            BoxBlur.Shader = _box;
            Invert.Shader = _invert;

            Coloring.Shader = _coloring;
            Mix.Shader = _colorMixer;
        }
    }
}
