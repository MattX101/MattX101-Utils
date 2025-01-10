using UnityEngine;
using Utils.Noise.Profiles;

namespace Utils.Noise
{
    internal static class FastNoise2DCPU
    {
        private const float DefaultCanvasSize = 1000.0f;

        private static int _resX, _resY;
        private static float _halfResX, _halfResY;

        private static float ResToLengthRatio => DefaultCanvasSize / _resX;
        private static float ResToWidthRatio => DefaultCanvasSize / _resY;

        internal static void GenerateNoise2D(ref float[] noiseMap, int resX, int resY, bool is3D, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            _resX = resX;
            _resY = resY;
            _halfResX = _resX / 2.0f;
            _halfResY = _resY / 2.0f;

            if (noiseProfile.warp)
            {
                if (warpProfile == null)
                    return;

                if (is3D)
                {
                    WarpedNoise3D(ref noiseMap, noiseProfile, warpProfile);
                }
                else
                {
                    WarpedNoise2D(ref noiseMap, noiseProfile, warpProfile);
                }
            }
            else
            {
                if (is3D)
                {
                    Noise3D(ref noiseMap, noiseProfile);
                }
                else
                {
                    Noise2D(ref noiseMap, noiseProfile);
                }
            }
        }

        private static void Noise2D(ref float[] noiseMap, NoiseProfile noiseProfile)
        {
            Vector2 noiseScale = GetNoiseScale(noiseProfile);

            float startNoiseX = NoisePosX(0, noiseScale.x, noiseProfile.offset.x);
            float startNoiseY = NoisePosY(0, noiseScale.y, noiseProfile.offset.y);

            float xNoisePos = startNoiseX;
            float yNoisePos = startNoiseY;

            float xNoiseInc = 1.0f / noiseScale.x;
            float yNoiseInc = 1.0f / noiseScale.y;

            for (int y = 0, i = 0; y < _resY; y++)
            {
                for (int x = 0; x < _resX; x++, i++)
                {
                    noiseMap[i] = noiseProfile.GetNoise2D(xNoisePos, yNoisePos);
                    xNoisePos += xNoiseInc;
                }

                xNoisePos = startNoiseX;
                yNoisePos += yNoiseInc;
            }
        }

        private static void Noise3D(ref float[] noiseMap, NoiseProfile noiseProfile)
        {
            Vector2 noiseScale = GetNoiseScale(noiseProfile);

            float startNoiseX = NoisePosX(0, noiseScale.x, noiseProfile.offset.x);
            float startNoiseY = NoisePosY(0, noiseScale.y, noiseProfile.offset.y);

            float xNoisePos = startNoiseX;
            float yNoisePos = startNoiseY;

            float xNoiseInc = 1.0f / noiseScale.x;
            float yNoiseInc = 1.0f / noiseScale.y;

            for (int y = 0, i = 0; y < _resY; y++)
            {
                for (int x = 0; x < _resX; x++, i++)
                {
                    noiseMap[i] = noiseProfile.GetNoise3D(xNoisePos, yNoisePos, noiseProfile.offset.z);
                    xNoisePos += xNoiseInc;
                }

                xNoisePos = startNoiseX;
                yNoisePos += yNoiseInc;
            }
        }

        private static void WarpedNoise2D(ref float[] noiseMap, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            Vector2 noiseScale = GetNoiseScale(noiseProfile);
            Vector2 warpScale = GetWarpScale(warpProfile);

            float startNoiseX = NoisePosX(0, noiseScale.x, noiseProfile.offset.x);
            float startNoiseY = NoisePosY(0, noiseScale.y, noiseProfile.offset.y);
            float startWarpX = WarpPosX(0, warpScale.x, warpProfile.offset.x);
            float startWarpY = WarpPosY(0, warpScale.y, warpProfile.offset.y);

            float xNoisePos = startNoiseX;
            float yNoisePos = startNoiseY;
            float xWarpPos = startWarpX;
            float yWarpPos = startWarpY;

            float xNoiseInc = 1.0f / noiseScale.x;
            float yNoiseInc = 1.0f / noiseScale.y;
            float xWarpInc = 1.0f / warpScale.x;
            float yWarpInc = 1.0f / warpScale.y;

            for (int y = 0, i = 0; y < _resY; y++)
            {
                for (int x = 0; x < _resX; x++, i++)
                {
                    noiseMap[i] = noiseProfile.GetNoise2D(
                        xWarpPos + warpProfile.GetWarp2D(xNoisePos, yNoisePos),
                        yWarpPos + warpProfile.GetWarp2D(yNoisePos, xNoisePos));

                    xNoisePos += xNoiseInc;
                    xWarpPos += xWarpInc;
                }

                xNoisePos = startNoiseX;
                xWarpPos = startWarpX;

                yNoisePos += yNoiseInc;
                yWarpPos += yWarpInc;
            }
        }

        private static void WarpedNoise3D(ref float[] noiseMap, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            Vector2 noiseScale = GetNoiseScale(noiseProfile);
            Vector2 warpScale = GetWarpScale(warpProfile);

            float startNoiseX = NoisePosX(0, noiseScale.x, noiseProfile.offset.x);
            float startNoiseY = NoisePosY(0, noiseScale.y, noiseProfile.offset.y);
            float startWarpX = WarpPosX(0, warpScale.x, warpProfile.offset.x);
            float startWarpY = WarpPosY(0, warpScale.y, noiseProfile.offset.y);

            float xNoisePos = startNoiseX;
            float yNoisePos = startNoiseY;
            float xWarpPos = startWarpX;
            float yWarpPos = startWarpY;

            float xNoiseInc = 1.0f / noiseScale.x;
            float yNoiseInc = 1.0f / noiseScale.y;
            float xWarpInc = 1.0f / warpScale.x;
            float yWarpInc = 1.0f / warpScale.y;

            for (int y = 0, i = 0; y < _resY; y++)
            {
                for (int x = 0; x < _resX; x++, i++)
                {
                    noiseMap[i] = noiseProfile.GetNoise3D(
                        xWarpPos + warpProfile.GetWarp3D(xNoisePos, yNoisePos, warpProfile.offset.z),
                        yWarpPos + warpProfile.GetWarp3D(yNoisePos, xNoisePos, warpProfile.offset.z),
                        noiseProfile.offset.z);

                    xNoisePos += xNoiseInc;
                    xWarpPos += xWarpInc;
                }

                xNoisePos = startNoiseX;
                xWarpPos = startWarpX;

                yNoisePos += yNoiseInc;
                yWarpPos += yWarpInc;
            }
        }

        private static Vector2 GetNoiseScale(NoiseProfile noiseProfile)
        {
            return new Vector2(
                noiseProfile.scale.x / noiseProfile.universalScale / ResToLengthRatio,
                noiseProfile.scale.y / noiseProfile.universalScale / ResToWidthRatio);
        }

        private static Vector2 GetWarpScale(WarpProfile warpProfile)
        {
            return new Vector2(
                warpProfile.scale.x / warpProfile.universalScale / ResToLengthRatio,
                warpProfile.scale.y / warpProfile.universalScale / ResToWidthRatio);
        }

        private static int Index(int x, int y) => y * _resX + x;

        private static float NoisePosX(float x, float scale, float offset) => ((x - _halfResX) / scale) + offset;
        private static float NoisePosY(float y, float scale, float offset) => ((y - _halfResY) / scale) + offset;

        private static float WarpPosX(float x, float scale, float offset) => ((x - _halfResX) / scale) + offset;
        private static float WarpPosY(float y, float scale, float offset) => ((y - _halfResY) / scale) + offset;
    }
}
