using Utils.Noise.Profiles;

namespace Utils.Noise
{
    public static partial class FastNoise2D
    {
        private const float DefaultCanvasSize = 1000.0f;

        private static int _width, _height;
        private static float _halfWidth, _halfHeight;

        private static float _canvasToWidthRatio = 1.0f;
        private static float CanvasToWidthRatio => _canvasToWidthRatio;

        private static float _canvasToHeightRatio = 1.0f;
        private static float CanvasToHeightRatio => _canvasToHeightRatio;

        private static float _noiseScaleX = 1.0f, _noiseScaleY = 1.0f, _warpScaleX = 1.0f, _warpScaleY = 1.0f;

        private static float[] _xNoisePoints, _yNoisePoints, _xWarpPoints, _yWarpPoints;

        private static void Init(int width, int height)
        {
            _width = width;
            _height = height;
            _halfWidth = _width / 2.0f;
            _halfHeight = _height / 2.0f;

            _canvasToWidthRatio = DefaultCanvasSize / _width;
            _canvasToHeightRatio = DefaultCanvasSize / _height;
        }

        private static void Init(int width, int height, NoiseProfile noiseProfile, WarpProfile warpProfile)
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

            SetPoints(noiseProfile, warpProfile);
        }

        private static void SetPoints(NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            for (int x = 0; x < _width; x++)
            {
                _xNoisePoints[x] = ((x - _halfWidth) / _noiseScaleX) + noiseProfile.Offset.x;
            }

            for (int y = 0; y < _height; y++)
            {
                _yNoisePoints[y] = ((y - _halfHeight) / _noiseScaleY) + noiseProfile.Offset.y;
            }

            if (warpProfile != null)
            {
                for (int x = 0; x < _width; x++)
                {
                    _xWarpPoints[x] = (_xNoisePoints[x] / _warpScaleX) + warpProfile.Offset.x;
                    _xWarpPoints[x] /= CanvasToWidthRatio;
                }

                for (int y = 0; y < _height; y++)
                {
                    _yWarpPoints[y] = (_yNoisePoints[y] / _warpScaleY) + warpProfile.Offset.y;
                    _yWarpPoints[y] /= CanvasToHeightRatio;
                }
            }
        }

        public static float Single(NoiseProfile noiseProfile, float noiseX, float noiseY)
        {
            return noiseProfile.GetNoise2D(noiseX, noiseY);
        }

        public static float Single(NoiseProfile noiseProfile, float noiseX, float noiseY, float offsetZ)
        {
            return noiseProfile.GetNoise3D(noiseX, noiseY, offsetZ);
        }

        public static float Single(NoiseProfile noiseProfile, WarpProfile warpProfile, float noiseX, float noiseY, float warpX, float warpY)
        {
            return Single(
                noiseProfile,
                noiseX + warpProfile.GetWarp2D(warpX, warpY),
                noiseY + warpProfile.GetWarp2D(warpY, warpX));
        }

        public static float Single(NoiseProfile noiseProfile, WarpProfile warpProfile, float noiseX, float noiseY, float offsetZ, float warpX, float warpY, float warpOffsetZ)
        {
            return Single(
                noiseProfile,
                noiseX + warpProfile.GetWarp3D(warpX, warpY, warpOffsetZ),
                noiseY + warpProfile.GetWarp3D(warpY, warpX, warpOffsetZ),
                offsetZ);
        }

        public static void Generate(ref float[] noiseMap, int width, int height, bool is3D, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            Init(width, height, noiseProfile, warpProfile);

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

        private static void Noise2D(ref float[] noiseMap, NoiseProfile noiseProfile)
        {
            for (int y = 0, i = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++, i++)
                {
                    noiseMap[i] = Single(noiseProfile, _xNoisePoints[x], _yNoisePoints[y]);
                }
            }
        }

        private static void Noise3D(ref float[] noiseMap, NoiseProfile noiseProfile)
        {
            for (int y = 0, i = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++, i++)
                {
                    noiseMap[i] = Single(noiseProfile, _xNoisePoints[x], _yNoisePoints[y], noiseProfile.Offset.z);
                }
            }
        }

        private static void WarpedNoise2D(ref float[] noiseMap, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            for (int y = 0, i = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++, i++)
                {
                    noiseMap[i] = Single(noiseProfile, warpProfile, _xNoisePoints[x], _yNoisePoints[y], _xWarpPoints[x], _yWarpPoints[y]);
                }
            }
        }

        private static void WarpedNoise3D(ref float[] noiseMap, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            for (int y = 0, i = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++, i++)
                {
                    noiseMap[i] = Single(noiseProfile, warpProfile, _xNoisePoints[x], _yNoisePoints[y], noiseProfile.Offset.z, _xWarpPoints[x], _yWarpPoints[y], warpProfile.Offset.z);
                }
            }
        }

        private static float GetNoiseScaleX(NoiseProfile profile) => profile.Scale.x / profile.UniversalScale / CanvasToWidthRatio;
        private static float GetNoiseScaleY(NoiseProfile profile) => profile.Scale.y / profile.UniversalScale / CanvasToHeightRatio;

        private static float GetWarpScaleX(WarpProfile profile) => profile.Scale.x / profile.UniversalScale / CanvasToWidthRatio;
        private static float GetWarpScaleY(WarpProfile profile) => profile.Scale.y / profile.UniversalScale / CanvasToHeightRatio;
    }
}
