namespace Utils.Colors.Model
{
    public struct HSV
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

        private float _value;
        public float Value
        {
            readonly get => _value;
            internal set => _value = value;
        }

        public HSV(float hue, float saturation, float value)
        {
            _hue = hue;
            _saturation = saturation;
            _value = value;
        }
    }
}
