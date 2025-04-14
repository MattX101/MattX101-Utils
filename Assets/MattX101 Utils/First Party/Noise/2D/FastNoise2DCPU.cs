using Utils.Noise.Profiles;
using UnityEngine;

namespace Utils.Noise
{
    public static partial class FastNoise2D
    {
        private const float DefaultCanvasSize = 1000.0f;

        private static float _canvasToScreenRatio = 1.0f;
        private static float CanvasToScreenRatio => _canvasToScreenRatio;

        private static float _canvasToWidthRatio, _canvasToHeightRatio;
        private static float CanvasToWidthRatio => _canvasToWidthRatio;
        private static float CanvasToHeightRatio => _canvasToHeightRatio;

        private static int _width, _height;
        private static float _halfWidth, _halfHeight;

        private static float _noiseScaleX = 1.0f, _noiseScaleY = 1.0f, _warpScaleX = 1.0f, _warpScaleY = 1.0f;

        private static float[] _xNoisePoints, _yNoisePoints, _xWarpPoints, _yWarpPoints;

        private static float _sin, _cos;

        private static void Init(int width, int height)
        {
            _width = width;
            _height = height;
            _halfWidth = _width / 2.0f;
            _halfHeight = _height / 2.0f;

            _canvasToWidthRatio = DefaultCanvasSize / _width;
            _canvasToHeightRatio = DefaultCanvasSize / _height;

            _canvasToScreenRatio =
                CanvasToWidthRatio > CanvasToHeightRatio ?
                CanvasToWidthRatio :
                CanvasToHeightRatio;
        }

        private static void Init(int width, int height, NoiseProfile noiseProfile, WarpProfile warpProfile = null, bool setPoints = true)
        {
            Init(width, height);

            _noiseScaleX = GetNoiseScaleX(noiseProfile);
            _noiseScaleY = GetNoiseScaleY(noiseProfile);

            _xNoisePoints = new float[_width];
            _yNoisePoints = new float[_height];

            if (warpProfile != null)
            {
                _warpScaleX = GetWarpScaleX(warpProfile);
                _warpScaleY = GetWarpScaleY(warpProfile);

                _xWarpPoints = new float[_width];
                _yWarpPoints = new float[_height];
            }

            if (setPoints)
            {
                SetPoints(warpProfile);
            }
        }

        private static void SetPoints(WarpProfile warpProfile = null)
        {
            for (int x = 0; x < _width; x++)
            {
                _xNoisePoints[x] = (x - _halfWidth) / _noiseScaleX;
            }

            for (int y = 0; y < _height; y++)
            {
                _yNoisePoints[y] = (y - _halfHeight) / _noiseScaleY;
            }

            if (warpProfile != null)
            {
                for (int x = 0; x < _width; x++)
                {
                    _xWarpPoints[x] = (_xNoisePoints[x] / _warpScaleX) + warpProfile.Offset.x;
                    _xWarpPoints[x] /= CanvasToScreenRatio;
                }

                for (int y = 0; y < _height; y++)
                {
                    _yWarpPoints[y] = (_yNoisePoints[y] / _warpScaleY) + warpProfile.Offset.y;
                    _yWarpPoints[y] /= CanvasToScreenRatio;
                }
            }
        }

        public static void Generate(ref float[] noiseMap, int width, int height, bool is3D, NoiseProfile noiseProfile, WarpProfile warpProfile = null)
        {
            CalculateAngles(noiseProfile.Roll);

            Init(width, height, noiseProfile, warpProfile);

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

        private static float GetNoiseScaleX(NoiseProfile profile) => profile.Scale.x / profile.UniversalScale / CanvasToScreenRatio;
        private static float GetNoiseScaleY(NoiseProfile profile) => profile.Scale.y / profile.UniversalScale / CanvasToScreenRatio;

        private static float GetWarpScaleX(WarpProfile profile) => profile.Scale.x / profile.UniversalScale / CanvasToScreenRatio;
        private static float GetWarpScaleY(WarpProfile profile) => profile.Scale.y / profile.UniversalScale / CanvasToScreenRatio;

        internal static float RotateX(float x, float y) => x * _sin + y * _cos;
        internal static float RotateY(float x, float y) => x * _cos - y * _sin;

        private static void CalculateAngles(float roll)
        {
            _sin = Mathf.Sin(roll * Mathf.Deg2Rad);
            _cos = Mathf.Cos(roll * Mathf.Deg2Rad);
        }
    }
}
