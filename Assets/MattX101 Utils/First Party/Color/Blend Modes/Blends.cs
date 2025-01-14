namespace Utils.Colors.Blend
{
    public enum Blends
    {
        /// Arithmetic
        Add,
        Subtract,
        Multiply,
        Divide,
        Average,

        /// Darken
        Darken,
        ColorBurn,
        LinearBurn,
        GammaDark,

        /// Lighten
        Lighten,
        Shine,
        ColorDodge,
        Screen,
        Overlay,
        SoftLight,
        HardLight,
        //VividLight,
        LinearLight,
        //PinLight,
        HardMix,
        Tint,
        GammaLight,
        GammaIllumination,

        /// Other
        Exclusion,
        Difference,
        Negation,

        ///
        Hue,
        Saturation,
        Color,
        Luminosity
    }
}
