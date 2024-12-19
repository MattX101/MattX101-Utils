namespace Utils.Curves
{
    public static class Bias
    {
        public static float Calculate(float time, float bias)
        {
            return time / (((1.0f / bias) - 2.0f) * (1.0f - time) + 1.0f);
        }
    }
}
