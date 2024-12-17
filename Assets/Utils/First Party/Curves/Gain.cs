namespace Utils.Curves
{
    public static class Gain
    {
        public static float Calculate(float time, float gain)
        {
            return 
                time < 0.5f ? 
                Bias.Calculate(time * 2, gain) / 2 :
                Bias.Calculate(time * 2 - 1, 1 - gain) / 2.0f + 0.5f;
        }
    }
}
