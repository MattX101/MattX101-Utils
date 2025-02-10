using Utils.Noise.Profiles;

namespace Utils.Noise
{
    public static partial class FastNoise2D
    {
        private const float DefaultCanvasSize = 1000.0f;

        private static int _resX, _resY;
        private static float _halfResX, _halfResY;

        private static float _resToLengthRatio = 1.0f;
        private static float ResToLengthRatio => _resToLengthRatio;

        private static float _resToWidthRatio = 1.0f;
        private static float ResToWidthRatio => _resToWidthRatio;

        private static float _noiseScaleX = 1.0f, _noiseScaleY = 1.0f, _warpScaleX = 1.0f, _warpScaleY = 1.0f;
        private static float _startNoiseX = 0.0f, _startNoiseY = 0.0f, _startWarpX = 0.0f, _startWarpY = 0.0f;
        private static float _xNoisePos = 0.0f, _yNoisePos = 0.0f, _xWarpPos = 0.0f, _yWarpPos = 0.0f;
        private static float _xNoiseInc = 1.0f, _yNoiseInc = 1.0f, _xWarpInc = 1.0f, _yWarpInc = 1.0f;

        private static void Init()
        {
            _resToLengthRatio = DefaultCanvasSize / _resX;
            _resToWidthRatio  = DefaultCanvasSize / _resY;
        }

        private static void Init(NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            Init();

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

        public static void Generate(ref float[] noiseMap, int resX, int resY, bool is3D, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            _resX = resX;
            _resY = resY;
            _halfResX = _resX / 2.0f;
            _halfResY = _resY / 2.0f;

            Init(noiseProfile, warpProfile);

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
            for (int y = 0, i = 0; y < _resY; y++)
            {
                for (int x = 0; x < _resX; x++, i++)
                {
                    noiseMap[i] = noiseProfile.GetNoise2D(_xNoisePos, _yNoisePos);
                    _xNoisePos += _xNoiseInc;
                }

                _xNoisePos = _startNoiseX;
                _yNoisePos += _yNoiseInc;
            }
        }

        private static void Noise3D(ref float[] noiseMap, NoiseProfile noiseProfile)
        {
            for (int y = 0, i = 0; y < _resY; y++)
            {
                for (int x = 0; x < _resX; x++, i++)
                {
                    noiseMap[i] = noiseProfile.GetNoise3D(_xNoisePos, _yNoisePos, noiseProfile.Offset.z);
                    _xNoisePos += _xNoiseInc;
                }

                _xNoisePos = _startNoiseX;
                _yNoisePos += _yNoiseInc;
            }
        }

        private static void WarpedNoise2D(ref float[] noiseMap, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            for (int y = 0, i = 0; y < _resY; y++)
            {
                for (int x = 0; x < _resX; x++, i++)
                {
                    noiseMap[i] = noiseProfile.GetNoise2D(
                        _xWarpPos + warpProfile.GetWarp2D(_xNoisePos, _yNoisePos),
                        _yWarpPos + warpProfile.GetWarp2D(_yNoisePos, _xNoisePos));

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
            for (int y = 0, i = 0; y < _resY; y++)
            {
                for (int x = 0; x < _resX; x++, i++)
                {
                    noiseMap[i] = noiseProfile.GetNoise3D(
                        _xWarpPos + warpProfile.GetWarp3D(_xNoisePos, _yNoisePos, warpProfile.Offset.z),
                        _yWarpPos + warpProfile.GetWarp3D(_yNoisePos, _xNoisePos, warpProfile.Offset.z),
                        noiseProfile.Offset.z);

                    _xNoisePos += _xNoiseInc;
                    _xWarpPos += _xWarpInc;
                }

                _xNoisePos = _startNoiseX;
                _xWarpPos = _startWarpX;

                _yNoisePos += _yNoiseInc;
                _yWarpPos += _yWarpInc;
            }
        }

        private static float GetNoiseScaleX(NoiseProfile profile) => profile.Scale.x / profile.UniversalScale / ResToLengthRatio;
        private static float GetNoiseScaleY(NoiseProfile profile) => profile.Scale.y / profile.UniversalScale / ResToWidthRatio;

        private static float GetWarpScaleX(WarpProfile profile) => profile.Scale.x / profile.UniversalScale / ResToLengthRatio;
        private static float GetWarpScaleY(WarpProfile profile) => profile.Scale.y / profile.UniversalScale / ResToWidthRatio;

        private static float NoisePosX(float x, float scale, float offset) => ((x - _halfResX) / scale) + offset;
        private static float NoisePosY(float y, float scale, float offset) => ((y - _halfResY) / scale) + offset;

        private static float WarpPosX(float x, float scale, float offset) => ((x - _halfResX) / scale) + offset;
        private static float WarpPosY(float y, float scale, float offset) => ((y - _halfResY) / scale) + offset;
    }
}
