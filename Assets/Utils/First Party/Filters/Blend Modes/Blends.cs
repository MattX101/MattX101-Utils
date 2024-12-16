namespace Utils.Filters.Blend
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
        ColourBurn,
        LinearBurn,
        GammaDark,

        /// Lighten
        Lighten,
        Shine,
        ColourDodge,
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
