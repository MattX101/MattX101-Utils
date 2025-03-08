using Utils.Noise.Profiles;

namespace Utils.Noise
{
    public static partial class FastNoise2D
    {
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
    }
}
