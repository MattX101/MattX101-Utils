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
        private static float _startNoiseX = 0.0f, _startNoiseY = 0.0f, _startWarpX = 0.0f, _startWarpY = 0.0f;
        private static float _xNoisePos = 0.0f, _yNoisePos = 0.0f, _xWarpPos = 0.0f, _yWarpPos = 0.0f;
        private static float _xNoiseInc = 1.0f, _yNoiseInc = 1.0f, _xWarpInc = 1.0f, _yWarpInc = 1.0f;

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
            _startNoiseX = NoisePosX(0, _noiseScaleX, noiseProfile.Offset.x);
            _startNoiseY = NoisePosY(0, _noiseScaleY, noiseProfile.Offset.y);
            _xNoisePos = _startNoiseX;
            _yNoisePos = _startNoiseY;
            _xNoiseInc = 1.0f / _noiseScaleX;
            _yNoiseInc = 1.0f / _noiseScaleY;

            if (warpProfile == null)
                return;

            _warpScaleX = GetWarpScaleX(warpProfile);
            _warpScaleY = GetWarpScaleY(warpProfile);
            _startWarpX = WarpPosX(0, _warpScaleX, warpProfile.Offset.x);
            _startWarpY = WarpPosY(0, _warpScaleY, warpProfile.Offset.y);
            _xWarpPos = _startWarpX;
            _yWarpPos = _startWarpY;
            _xWarpInc = 1.0f / _warpScaleX;
            _yWarpInc = 1.0f / _warpScaleY;
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
                    noiseMap[i] = Single(noiseProfile, _xNoisePos, _yNoisePos);
                    _xNoisePos += _xNoiseInc;
                }

                _xNoisePos = _startNoiseX;
                _yNoisePos += _yNoiseInc;
            }
        }

        private static void Noise3D(ref float[] noiseMap, NoiseProfile noiseProfile)
        {
            for (int y = 0, i = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++, i++)
                {
                    noiseMap[i] = Single(noiseProfile, _xNoisePos, _yNoisePos, noiseProfile.Offset.z);
                    _xNoisePos += _xNoiseInc;
                }

                _xNoisePos = _startNoiseX;
                _yNoisePos += _yNoiseInc;
            }
        }

        private static void WarpedNoise2D(ref float[] noiseMap, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            for (int y = 0, i = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++, i++)
                {
                    noiseMap[i] = Single(noiseProfile, warpProfile, _xNoisePos, _yNoisePos, _xWarpPos, _yWarpPos);

                    _xNoisePos += _xNoiseInc;
                    _xWarpPos += _xWarpInc;
                }

                _xNoisePos = _startNoiseX;
                _xWarpPos = _startWarpX;

                _yNoisePos += _yNoiseInc;
                _yWarpPos += _yWarpInc;
            }
        }

        private static void WarpedNoise3D(ref float[] noiseMap, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            for (int y = 0, i = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++, i++)
                {
                    noiseMap[i] = Single(noiseProfile, warpProfile, _xNoisePos, _yNoisePos, noiseProfile.Offset.z, _xWarpPos, _yWarpPos, warpProfile.Offset.z);

                    _xNoisePos += _xNoiseInc;
                    _xWarpPos += _xWarpInc;
                }

                _xNoisePos = _startNoiseX;
                _xWarpPos = _startWarpX;

                _yNoisePos += _yNoiseInc;
                _yWarpPos += _yWarpInc;
            }
        }

        private static float GetNoiseScaleX(NoiseProfile profile) => profile.Scale.x / profile.UniversalScale / CanvasToWidthRatio;
        private static float GetNoiseScaleY(NoiseProfile profile) => profile.Scale.y / profile.UniversalScale / CanvasToHeightRatio;

        private static float GetWarpScaleX(WarpProfile profile) => profile.Scale.x / profile.UniversalScale / CanvasToWidthRatio;
        private static float GetWarpScaleY(WarpProfile profile) => profile.Scale.y / profile.UniversalScale / CanvasToHeightRatio;

        private static float NoisePosX(float x, float scale, float offset) => ((x - _halfWidth) / scale) + offset;
        private static float NoisePosY(float y, float scale, float offset) => ((y - _halfHeight) / scale) + offset;

        private static float WarpPosX(float x, float scale, float offset) => ((x - _halfWidth) / scale) + offset;
        private static float WarpPosY(float y, float scale, float offset) => ((y - _halfHeight) / scale) + offset;
    }
}
