#include "../Libraries/FastNoiseLite.hlsl"

// Non-Linear

float Single(fnl_state noiseState, float x, float y)
{
    return fnlGetNoise2D(noiseState, x, y);
}

float Single(fnl_state noiseState, float x, float y, float z)
{
    return fnlGetNoise3D(noiseState, x, y, z);
}

float Single(fnl_state noiseState, float noiseX, float noiseY, fnl_state warpState, float warpX, float warpY, float amp)
{
    return Single(
        noiseState,
        noiseX + fnlGetNoise2D(warpState, warpX, warpY) * amp,
        noiseY + fnlGetNoise2D(warpState, warpY, warpX) * amp);
}

float Single(fnl_state noiseState, float noiseX, float noiseY, float noiseZ, fnl_state warpState, float warpX, float warpY, float warpZ, float amp)
{
    return Single(
        noiseState,
        noiseX + fnlGetNoise3D(warpState, warpX, warpY, warpZ) * amp,
        noiseY + fnlGetNoise3D(warpState, warpY, warpX, warpZ) * amp,
        noiseZ);
}

// Linear

float Single_Linear(fnl_state noiseState, float x, float y)
{
    return (Single(noiseState, x, y) + 1) / 2;
}

float Single_Linear(fnl_state noiseState, float x, float y, float z)
{
    return (Single(noiseState, x, y, z) + 1) / 2;
}

float Single_Linear(fnl_state noiseState, float noiseX, float noiseY, fnl_state warpState, float warpX, float warpY, float amp)
{
    return Single_Linear(
        noiseState,
        noiseX + fnlGetNoise2D(warpState, warpX, warpY) * amp,
        noiseY + fnlGetNoise2D(warpState, warpY, warpX) * amp);
}

float Single_Linear(fnl_state noiseState, float noiseX, float noiseY, float noiseZ, fnl_state warpState, float warpX, float warpY, float warpZ, float amp)
{
    return Single_Linear(
        noiseState,
        noiseX + fnlGetNoise3D(warpState, warpX, warpY, warpZ) * amp,
        noiseY + fnlGetNoise3D(warpState, warpY, warpX, warpZ) * amp,
        noiseZ);
}