// Utils
bool LessThan(float v)
{
    return v <= 0.5f;
}

float Sqrt(float v)
{
    return sqrt(v);
}

float Invert(float v)
{
    return 1.0f - v;
}

float TimesTwo(float v)
{
    return v * 2;
}

float Abs(float v)
{
    return abs(v);
}

float Min(float a, float b)
{
    return min(a, b);
}

float Max(float a, float b)
{
    return max(a, b);
}

float Power(float v, float p = 2.0f)
{
    return pow(v, p);
}

// Arithmetic
float Add(float a, float b)
{
    return a + b;
}

float Subtract(float a, float b)
{
    return a - b;
}

float Multiply(float a, float b)
{
    return a * b;
}

float Divide(float a, float b)
{
    return a / b;
}

float Average(float a, float b)
{
    return Divide(Add(a, b), 2);
}

// Darken
float Darken(float a, float b)
{
    return a < b ? a : b;
}

float ColorBurn(float a, float b)
{
    return Invert(Divide(Invert(a), b));
}

float LinearBurn(float a, float b)
{
    return Subtract(Add(a, b), 1);
}

float GammaDark(float a, float b)
{
    return Power(a, Divide(1, b));
}

// Lighten
float Lighten(float a, float b)
{
    return a > b ? a : b;
}

float Shine(float a, float b)
{
    return Add(Power(b), a);
}

float Screen(float a, float b)
{
    return Invert(Multiply(Invert(a), Invert(b)));
}

float ColorDodge(float a, float b)
{
    return Divide(a, Invert(b));
}

float Overlay(float a, float b)
{
    return
    LessThan(a) ?
    TimesTwo(Multiply(a, b)) :
    Invert(TimesTwo(Multiply(Invert(a), Invert(b))));
}

float SoftLight(float a, float b)
{
    return
    LessThan(a) ?
    Multiply(a, Add(b, 0.5f)) :
    Invert(Invert(a) * Invert(Subtract(b, 0.5f)));
}

float HardLight(float a, float b)
{
    return
    LessThan(b) ?
    Multiply(b, TimesTwo(a)) :
    Invert(Multiply(Invert(b), Invert(TimesTwo(Subtract(a, 0.5f)))));
}

float LinearLight(float a, float b)
{
    return
    LessThan(a) ?
    Subtract(Add(a, TimesTwo(b)), 1.0f) :
    Add(a, TimesTwo(Subtract(b, 0.5f)));
}

float HardMix(float a, float b)
{
    return Add(a, b) < 1 ? 0 : 1;
}

float Tint(float a, float b)
{
    return Invert(Subtract(b, a));
}

float GammaLight(float a, float b)
{
    return Power(a, Invert(b));
}

float GammaIllumination(float a, float b)
{
    return Invert(GammaDark(a, b));
}

// Other

float Exclusion(float a, float b)
{
    return Subtract(Add(b, a), TimesTwo(Multiply(a, b)));
}

float Difference(float a, float b)
{
    return Abs(b - a);
}

float Negation(float a, float b)
{
    float v = Add(a, b);
    return v > 1 ? Add(1.0f, Invert(v)) : v;
}
