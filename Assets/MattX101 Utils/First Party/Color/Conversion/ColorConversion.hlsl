#include "../Models/Models.hlsl"

float Min(float3 c)
{
    return min(min(c.x, c.y), c.z);
}

float Max(float3 c)
{
    return max(max(c.x, c.y), c.z);
}

float Hue(float3 c, float min, float max, float diff)
{
    float h = 0.0f;
    if (diff > 0.0f)
    {
        if (max == c.x)
        {
            h = 60.0f * ((c.y - c.z) / diff);
            h = h < 0 ? h + 360 : h;
        }
        else if (max == c.y)
        {
            h = 60.0f * (((c.z - c.x) / diff) + 2);
        }
        else if (max == c.z)
        {
            h = 60.0f * (((c.x - c.y) / diff) + 4);
        }
    }

    return h;
}

float4 ReturnColor(float hue, float C, float X, float4 m)
{
    [flatten]
    switch (hue)
    {
        case hue >= 300: return float4(C, 0, X, 1) + m;
        case hue >= 240: return float4(X, 0, C, 1) + m;
        case hue >= 180: return float4(0, X, C, 1) + m;
        case hue >= 120: return float4(0, C, X, 1) + m;
        case hue >=  60: return float4(X, C, 0, 1) + m;
        case hue <   60: return float4(C, X, 0, 1) + m;
    }
}

// HSL
HSL RGBToHSL(float4 c)
{
    float min = Min(c.xyz);
    float max = Max(c.xyz);
    float diff = max - min;

    float l = (max + min) / 2.0f;
    
    HSL hsl;
    hsl.hue = Hue(c.xyz, min, max, diff);
    hsl.saturation = diff == 0.0f ? 0.0f : diff / (1.0f - abs(2.0f * l - 1.0f));
    hsl.lightness = l;
    
    return hsl;
}

float4 HSLToRGB(HSL hsl)
{
    float C = (1.0f - abs(2 * hsl.lightness - 1.0f)) * hsl.saturation;
    float X = C * (1.0f - abs((hsl.hue / 60.0f) % 2.0f - 1.0f));

    float n = hsl.lightness - C / 2.0f;
    
    return ReturnColor(hsl.hue, C, X, n);
}

// HSV
HSV RGBToHSV(float4 c)
{
    float min = Min(c.xyz);
    float max = Max(c.xyz);
    float diff = max - min;
    
    HSV hsv;
    hsv.hue = Hue(c.xyz, min, max, diff);
    hsv.saturation = max == 0 ? 0 : diff / max;
    hsv.value = max;
    
    return hsv;
}

float4 HSVToRGB(HSV hsv)
{
    float C = hsv.value * hsv.saturation;
    float X = C * (1.0f - abs((hsv.hue / 60.0f) % 2.0f - 1.0f));

    float n = hsv.value - C;
    
    return ReturnColor(hsv.hue, C, X, n);
}
