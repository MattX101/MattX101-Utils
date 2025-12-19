using Utils.Noise.Profiles;
using UnityEngine;

namespace Utils.Noise
{
    public static partial class FastNoise2D
    {
        private static float[] _xNoisePoints, _yNoisePoints, _xWarpPoints, _yWarpPoints;

        public static void Generate(ref float[] noiseMap, int width, int height, bool is3D, NoiseProfile noiseProfile, WarpProfile warpProfile = null)
        {
            if (_xNoisePoints == null || _xNoisePoints.Length != width)
            {
                _xNoisePoints = new float[width];
                _yNoisePoints = new float[height];
            }
            
            float canvasToScreenRatio = Mathf.Max(1000.0f / _xNoisePoints.Length, 1000.0f / _yNoisePoints.Length);

            float noiseScaleX = GetNoiseScaleX(noiseProfile, canvasToScreenRatio);
            float noiseScaleY = GetNoiseScaleY(noiseProfile, canvasToScreenRatio);

            if (warpProfile != null)
            {
                float warpScaleX = GetWarpScaleX(warpProfile, canvasToScreenRatio);
                float warpScaleY = GetWarpScaleY(warpProfile, canvasToScreenRatio);

                if (_xWarpPoints == null || _xWarpPoints.Length != width)
                {
                    _xWarpPoints = new float[width];
                    _yWarpPoints = new float[height];
                }

                SetPoints(noiseScaleX, noiseScaleY, warpProfile, warpScaleX, warpScaleY, canvasToScreenRatio);
            }
            else
            {
                SetPoints(noiseScaleX, noiseScaleY);
            }

            if (noiseProfile.Normalized)
            {
                if (noiseProfile.Warp)
                {
                    if (warpProfile == null)
                        return;

                    switch (is3D)
                    {
                        case false:
                            WarpedNoise2D_Linear(ref noiseMap, noiseProfile, warpProfile);
                            break;
                        case true:
                            WarpedNoise3D_Linear(ref noiseMap, noiseProfile, warpProfile);
                            break;
                    }
                }
                else
                {
                    switch (is3D)
                    {
                        case false:
                            Noise2D_Linear(ref noiseMap, noiseProfile);
                            break;
                        case true:
                            Noise3D_Linear(ref noiseMap, noiseProfile);
                            break;
                    }
                }
            }
            else
            {
                if (noiseProfile.Warp)
                {
                    if (warpProfile == null)
                        return;

                    switch (is3D)
                    {
                        case false:
                            WarpedNoise2D(ref noiseMap, noiseProfile, warpProfile);
                            break;
                        case true:
                            WarpedNoise3D(ref noiseMap, noiseProfile, warpProfile);
                            break;
                    }
                }
                else
                {
                    switch (is3D)
                    {
                        case false:
                            Noise2D(ref noiseMap, noiseProfile);
                            break;
                        case true:
                            Noise3D(ref noiseMap, noiseProfile);
                            break;
                    }
                }
            }
        }
        
        private static void SetPoints(float noiseScaleX, float noiseScaleY, WarpProfile warpProfile = null, float warpScaleX = 1, float warpScaleY = 1, float canvasToScreenRatio = 1)
        {
            float halfWidth = _xNoisePoints.Length / 2.0f;
            float halfHeight = _yNoisePoints.Length / 2.0f;

            if (((0 - halfWidth) / noiseScaleX) != _xNoisePoints[0])
            {
                for (int x = 0; x < _xNoisePoints.Length; x++)
                {
                    _xNoisePoints[x] = (x - halfWidth) / noiseScaleX;
                }
            }

            if (((0 - halfHeight) / noiseScaleY) != _yNoisePoints[0])
            {
                for (int y = 0; y < _yNoisePoints.Length; y++)
                {
                    _yNoisePoints[y] = (y - halfHeight) / noiseScaleY;
                }
            }

            if (warpProfile == null)
            {
                return;
            }

            if (((_xNoisePoints[0] / warpScaleX) + warpProfile.Offset.x) / canvasToScreenRatio != _xWarpPoints[0])
            {
                for (int x = 0; x < _xWarpPoints.Length; x++)
                {
                    _xWarpPoints[x] = (_xNoisePoints[x] / warpScaleX) + warpProfile.Offset.x;
                    _xWarpPoints[x] /= canvasToScreenRatio;
                }
            }

            if (((_yNoisePoints[0] / warpScaleY) + warpProfile.Offset.y) / canvasToScreenRatio != _yWarpPoints[0])
            {
                for (int y = 0; y < _yWarpPoints.Length; y++)
                {
                    _yWarpPoints[y] = (_yNoisePoints[y] / warpScaleY) + warpProfile.Offset.y;
                    _yWarpPoints[y] /= canvasToScreenRatio;
                }
            }
        }

        private static float GetNoiseScaleX(NoiseProfile profile, float canvasToScreenRatio) => profile.Scale.x * profile.UniversalScale / canvasToScreenRatio;
        private static float GetNoiseScaleY(NoiseProfile profile, float canvasToScreenRatio) => profile.Scale.y * profile.UniversalScale / canvasToScreenRatio;

        private static float GetWarpScaleX(WarpProfile profile, float canvasToScreenRatio) => profile.Scale.x * profile.UniversalScale / canvasToScreenRatio;
        private static float GetWarpScaleY(WarpProfile profile, float canvasToScreenRatio) => profile.Scale.y * profile.UniversalScale / canvasToScreenRatio;

        private static float RotateX(float x, float y, float sin, float cos) => x * sin + y * cos;
        private static float RotateY(float x, float y, float sin, float cos) => x * cos - y * sin;

        private static void CalculateAngles(ref float sin, ref float cos, float roll)
        {
            sin = Mathf.Sin(roll * Mathf.Deg2Rad);
            cos = Mathf.Cos(roll * Mathf.Deg2Rad);
        }
    }
}
