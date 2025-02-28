#include "../Libraries/FastNoiseLite.hlsl"

float Single(fnl_state noiseState, float x, float y)
{
    return (fnlGetNoise2D(noiseState, x, y) + 1) / 2;
}

float Single(fnl_state noiseState, float x, float y, float z)
{
    return (fnlGetNoise3D(noiseState, x, y, z) + 1) / 2;
}

float Single(fnl_state noiseState, float noiseX, float noiseY, fnl_state warpState, float warpX, float warpY, float amp)
{
    return (fnlGetNoise2D(
        noiseState,
        noiseX + fnlGetNoise2D(warpState, warpX, warpY) * amp,
        noiseY + fnlGetNoise2D(warpState, warpY, warpX) * amp)
        + 1) / 2;
}

float Single(fnl_state noiseState, float noiseX, float noiseY, float noiseZ, fnl_state warpState, float warpX, float warpY, float warpZ, float amp)
{
    return (fnlGetNoise3D(
        noiseState,
        noiseX + fnlGetNoise3D(warpState, warpX, warpY, warpZ) * amp,
        noiseY + fnlGetNoise3D(warpState, warpY, warpX, warpZ) * amp,
        noiseZ)
        + 1) / 2;
}