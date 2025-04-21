namespace Utils.Colors.Model
{
    public struct HSL
    {
        public float Hue { get; internal set; }
        public float Saturation { get; internal set; }
        public float Lightness { get; internal set; }

        public HSL(float hue, float saturation, float lightness)
        {
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
        }
    }
}
