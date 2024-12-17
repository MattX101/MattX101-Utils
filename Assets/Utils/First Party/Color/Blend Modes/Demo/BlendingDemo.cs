using UnityEngine;
using UnityEngine.UI;

namespace Utils.Colors.Blend
{
    public sealed class BlendingDemo : MonoBehaviour
    {
        [SerializeField] private Texture2D _noise1;
        [SerializeField] private Texture2D _noise2;

        [Space]

        [SerializeField] private RawImage _image;

        [Space]

        [SerializeField]
        private Blends _modes;
        private delegate Color BlendFormula(Color a, Color b);

        private void Update()
        {
            Color[] result = new Color[_noise1.width * _noise1.height];

            BlendFormula formula = GetFormula((int)_modes);
            for (int y = 0, i = 0; y < _noise1.height; y++)
            {
                for (int x = 0; x < _noise1.width; x++, i++)
                {
                    result[i] = formula(_noise1.GetPixel(x, y), _noise2.GetPixel(x, y));
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
                (int)Blends.Add => Mix.Add,
                (int)Blends.Subtract => Mix.Subtract,
                (int)Blends.Multiply => Mix.Multiply,
                (int)Blends.Divide => Mix.Divide,
                (int)Blends.Average => Mix.Average,

                /// Darken
                (int)Blends.Darken => Mix.Darken,
                (int)Blends.ColourBurn => Mix.ColorBurn,
                (int)Blends.LinearBurn => Mix.LinearBurn,
                (int)Blends.GammaDark => Mix.GammaDark,

                /// Lighten
                (int)Blends.Lighten => Mix.Lighten,
                (int)Blends.Shine => Mix.Shine,
                (int)Blends.ColourDodge => Mix.ColourDodge,
                (int)Blends.Screen => Mix.Screen,
                (int)Blends.Overlay => Mix.Overlay,
                (int)Blends.SoftLight => Mix.SoftLight,
                (int)Blends.HardLight => Mix.HardLight,
                // Vivid Light
                (int)Blends.LinearLight => Mix.LinearLight,
                // Pin Light
                (int)Blends.HardMix => Mix.HardMix,
                (int)Blends.Tint => Mix.Tint,
                (int)Blends.GammaLight => Mix.GammaLight,
                (int)Blends.GammaIllumination => Mix.GammaIllumination,

                /// Other
                (int)Blends.Exclusion => Mix.Exclusion,
                (int)Blends.Difference => Mix.Difference,
                (int)Blends.Negation => Mix.Negation,

                ///
                (int)Blends.Hue => Mix.Hue,
                (int)Blends.Saturation => Mix.Saturation,
                (int)Blends.Color => Mix.Color,
                (int)Blends.Luminosity => Mix.Luminosity,

                /// Default
                _ => Mix.Add
            };
        }
    }
}
