// Easing Functions
float EaseIn(float v, float p)
{
    return pow(v, p);
}

float EaseInCirc(float v, float p)
{
    return 1.0f - sqrt(1.0f - pow(v, p));
}

float EaseInOut(float v, float p)
{
    return
    v < 0.5f ?
    pow(2, p - 1) * pow(v, p) :
    1.0f - (pow(-2 * v + 2, p) / 2.0f);
}

float EaseInOutSine(float v, float p)
{
    return pow(-(cos(3.1415f * v) - 1) / 2.0f, p);
}

// Sin Functions
float Sine(float v, float p)
{
    return abs(pow(sin(3.1415f * v), p));
}

float SineSqrt(float v, float p)
{
    return sqrt(abs(pow(sin(3.1415f * v), p)));
}

float RepeatedSine(float v, float p)
{
    return
    v < 0.5f ?
    pow(sin(3.1415f * v), p) / 2.0f :
    1.0f + (-pow(sin(3.1415f * v), p) / 2.0f);
}

float SineFrequency(float v, float p)
{
    return sin(3.1415f * pow(v, p));
}

float RadianArcSine(float v, float p)
{
    return pow(asin(v * 0.841471f), p);
}

float RadianArcSineSqrt(float v, float p)
{
    return sqrt(pow(asin(v * 0.841471f), p));
}

float HalfDownUpSine(float v, float p)
{
    return
    v < 0.5f ?
    0.5f + (-pow(sin(3.1415f * v), p)) / 2.0f :
    1.0f + -pow(sin(3.1415f * v), p);
}