namespace Utils.Colors.Model
{
    public struct HSL
    {
        private float _hue;
        public float Hue
        {
            readonly get => _hue;
            internal set => _hue = value;
        }

        private float _saturation;
        public float Saturation
        {
            readonly get => _saturation;
            internal set => _saturation = value;
        }

        private float _lightness;
        public float Lightness
        {
            readonly get => _lightness;
            internal set => _lightness = value;
        }

        public HSL(float hue, float saturation, float lightness)
        {
            _hue = hue;
            _saturation = saturation;
            _lightness = lightness;
        }
    }
}
