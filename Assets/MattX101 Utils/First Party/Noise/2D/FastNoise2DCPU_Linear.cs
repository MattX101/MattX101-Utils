using Utils.Noise.Profiles;

namespace Utils.Noise
{
    public static partial class FastNoise2D
    {
        public static float Single_Linear(NoiseProfile noiseProfile, float noiseX, float noiseY)
        {
            return noiseProfile.GetNoise2D_Linear(
                noiseX + noiseProfile.Offset.x, 
                noiseY + noiseProfile.Offset.y
            );
        }

        public static float Single_Linear(NoiseProfile noiseProfile, float noiseX, float noiseY, float offsetZ)
        {
            return noiseProfile.GetNoise3D_Linear(
                noiseX + noiseProfile.Offset.x,
                noiseY + noiseProfile.Offset.y,
                offsetZ
            );
        }

        public static float Single_Linear(NoiseProfile noiseProfile, WarpProfile warpProfile, float noiseX, float noiseY, float warpX, float warpY)
        {
            return Single_Linear(
                noiseProfile,
                noiseX + warpProfile.GetWarp2D(warpX, warpY),
                noiseY + warpProfile.GetWarp2D(warpY, warpX));
        }

        public static float Single_Linear(NoiseProfile noiseProfile, WarpProfile warpProfile, float noiseX, float noiseY, float offsetZ, float warpX, float warpY, float warpOffsetZ)
        {
            return Single_Linear(
                noiseProfile,
                noiseX + warpProfile.GetWarp3D(warpX, warpY, warpOffsetZ),
                noiseY + warpProfile.GetWarp3D(warpY, warpX, warpOffsetZ),
                offsetZ);
        }

        private static void Noise2D_Linear(ref float[] noiseMap, NoiseProfile noiseProfile)
        {
            if (noiseProfile.ComputeRoll)
            {
                for (int y = 0, i = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++, i++)
                    {
                        noiseMap[i] = Single_Linear(
                            noiseProfile,
                            RotateX(_xNoisePoints[x], _yNoisePoints[y]),
                            RotateY(_xNoisePoints[x], _yNoisePoints[y])
                        );
                    }
                }
            }
            else
            {
                for (int y = 0, i = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++, i++)
                    {
                        noiseMap[i] = Single_Linear(
                            noiseProfile,
                            _xNoisePoints[x],
                            _yNoisePoints[y]
                        );
                    }
                }
            }
        }

        private static void Noise3D_Linear(ref float[] noiseMap, NoiseProfile noiseProfile)
        {
            if (noiseProfile.ComputeRoll)
            {
                for (int y = 0, i = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++, i++)
                    {
                        noiseMap[i] = Single_Linear(
                            noiseProfile,
                            RotateX(_xNoisePoints[x], _yNoisePoints[y]),
                            RotateY(_xNoisePoints[x], _yNoisePoints[y]),
                            noiseProfile.Offset.z
                        );
                    }
                }
            }
            else
            {
                for (int y = 0, i = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++, i++)
                    {
                        noiseMap[i] = Single_Linear(
                            noiseProfile,
                            _xNoisePoints[x],
                            _yNoisePoints[y],
                            noiseProfile.Offset.z
                        );
                    }
                }
            }
        }

        private static void WarpedNoise2D_Linear(ref float[] noiseMap, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            if (noiseProfile.ComputeRoll)
            {
                for (int y = 0, i = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++, i++)
                    {
                        noiseMap[i] = Single_Linear(
                            noiseProfile,
                            warpProfile,
                            RotateX(_xNoisePoints[x], _yNoisePoints[y]),
                            RotateY(_xNoisePoints[x], _yNoisePoints[y]),
                            RotateX(_xWarpPoints[x], _yWarpPoints[y]),
                            RotateY(_xWarpPoints[x], _yWarpPoints[y])
                        );
                    }
                }
            }
            else
            {
                for (int y = 0, i = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++, i++)
                    {
                        noiseMap[i] = Single_Linear(
                            noiseProfile,
                            warpProfile,
                            _xNoisePoints[x],
                            _yNoisePoints[y],
                            _xWarpPoints[x],
                            _yWarpPoints[y]
                        );
                    }
                }
            }
        }

        private static void WarpedNoise3D_Linear(ref float[] noiseMap, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            if (noiseProfile.ComputeRoll)
            {
                for (int y = 0, i = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++, i++)
                    {
                        noiseMap[i] = Single_Linear(
                            noiseProfile,
                            warpProfile,
                            RotateX(_xNoisePoints[x], _yNoisePoints[y]),
                            RotateY(_xNoisePoints[x], _yNoisePoints[y]),
                            noiseProfile.Offset.z,
                            RotateX(_xWarpPoints[x], _yWarpPoints[y]),
                            RotateY(_xWarpPoints[x], _yWarpPoints[y]),
                            warpProfile.Offset.z
                        );
                    }
                }
            }
            else
            {
                for (int y = 0, i = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++, i++)
                    {
                        noiseMap[i] = Single_Linear(
                            noiseProfile,
                            warpProfile,
                            _xNoisePoints[x],
                            _yNoisePoints[y],
                            noiseProfile.Offset.z,
                            _xWarpPoints[x],
                            _yWarpPoints[y],
                            warpProfile.Offset.z
                        );
                    }
                }
            }
        }
    }
}
