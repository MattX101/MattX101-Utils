using UnityEngine;
using UnityEngine.UI;

namespace Utils.Colors.Blend
{
    public sealed class BlendingDemo : MonoBehaviour
    {
        [Header("Shader"), SerializeField]
        private ComputeShader _blendShader;

        [SerializeField]
        private bool _useGPU = false;

        [Space]

        [SerializeField] private Texture2D _noise1;
        [SerializeField] private Texture2D _noise2;

        [Space]

        [SerializeField] private RawImage _image;

        [Space]

        [SerializeField]
        private Blends _modes;
        private delegate Color BlendFormula(Color a, Color b);

        private void Start()
        {
            MixGPU.Shader = _blendShader;
        }

        private void Update()
        {
            Color[] result = new Color[_noise1.width * _noise1.height];

            if (_useGPU)
            {
                ComputeBuffer resultBuffer = new ComputeBuffer(result.Length, sizeof(float) * 4);
                ComputeBuffer baseBuffer = new ComputeBuffer(result.Length, sizeof(float) * 4);
                ComputeBuffer blendBuffer = new ComputeBuffer(result.Length, sizeof(float) * 4);

                resultBuffer.SetData(_noise1.GetPixels());
                baseBuffer.SetData(_noise1.GetPixels());
                blendBuffer.SetData(_noise2.GetPixels());

                MixGPU.Blend(ref resultBuffer, baseBuffer, blendBuffer, _modes);

                resultBuffer.GetData(result);

                resultBuffer.Dispose();
                baseBuffer.Dispose();
                blendBuffer.Dispose();
            }
            else
            {
                BlendFormula formula = GetFormula((int)_modes);
                for (int y = 0, i = 0; y < _noise1.height; y++)
                {
                    for (int x = 0; x < _noise1.width; x++, i++)
                    {
                        result[i] = formula(_noise1.GetPixel(x, y), _noise2.GetPixel(x, y));
                    }
                }
            }

            Texture2D texture = new Texture2D(_noise1.width, _noise1.height);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            texture.SetPixels(result);
            texture.Apply();

            _image.texture = texture;
        }

        private BlendFormula GetFormula(int index)
        {
            return index switch
            {
                /// Arithmetic
                (int)Blends.Add => MixCPU.Add,
                (int)Blends.Subtract => MixCPU.Subtract,
                (int)Blends.Multiply => MixCPU.Multiply,
                (int)Blends.Divide => MixCPU.Divide,
                (int)Blends.Average => MixCPU.Average,

                /// Darken
                (int)Blends.Darken => MixCPU.Darken,
                (int)Blends.ColorBurn => MixCPU.ColorBurn,
                (int)Blends.LinearBurn => MixCPU.LinearBurn,
                (int)Blends.GammaDark => MixCPU.GammaDark,

                /// Lighten
                (int)Blends.Lighten => MixCPU.Lighten,
                (int)Blends.Shine => MixCPU.Shine,
                (int)Blends.ColorDodge => MixCPU.ColorDodge,
                (int)Blends.Screen => MixCPU.Screen,
                (int)Blends.Overlay => MixCPU.Overlay,
                (int)Blends.SoftLight => MixCPU.SoftLight,
                (int)Blends.HardLight => MixCPU.HardLight,
                // Vivid Light
                (int)Blends.LinearLight => MixCPU.LinearLight,
                // Pin Light
                (int)Blends.HardMix => MixCPU.HardMix,
                (int)Blends.Tint => MixCPU.Tint,
                (int)Blends.GammaLight => MixCPU.GammaLight,
                (int)Blends.GammaIllumination => MixCPU.GammaIllumination,

                /// Other
                (int)Blends.Exclusion => MixCPU.Exclusion,
                (int)Blends.Difference => MixCPU.Difference,
                (int)Blends.Negation => MixCPU.Negation,

                ///
                (int)Blends.Hue => MixCPU.Hue,
                (int)Blends.Saturation => MixCPU.Saturation,
                (int)Blends.Color => MixCPU.Color,
                (int)Blends.Luminosity => MixCPU.Luminosity,

                /// Default
                _ => MixCPU.Add
            };
        }
    }
}
