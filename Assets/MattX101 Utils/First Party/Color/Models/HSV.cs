namespace Utils.Colors.Model
{
    public struct HSV
    {
        public float Hue
        {
            get;
            internal set;
        }

        public float Saturation
        {
            get;
            internal set;
        }
        
        public float Value
        {
            get;
            internal set;
        }

        public HSV(float hue, float saturation, float value)
        {
            Hue = hue;
            Saturation = saturation;
            Value = value;
        }
    }
}
