using Utils.Noise.Profiles;

namespace Utils.Noise
{
    public static partial class FastNoise2D
    {
        private static float Single(NoiseProfile noiseProfile, float noiseX, float noiseY)
        {
            return noiseProfile.GetNoise2D(
                noiseX - noiseProfile.Offset.x,
                noiseY - noiseProfile.Offset.y
            );
        }

        private static float Single(NoiseProfile noiseProfile, float noiseX, float noiseY, float offsetZ)
        {
            return noiseProfile.GetNoise3D(
                noiseX - noiseProfile.Offset.x,
                noiseY - noiseProfile.Offset.y,
                offsetZ
            );
        }

        private static float Single(NoiseProfile noiseProfile, WarpProfile warpProfile, float noiseX, float noiseY, float warpX, float warpY)
        {
            return Single(
                noiseProfile,
                noiseX - warpProfile.GetWarp2D(warpX, warpY),
                noiseY - warpProfile.GetWarp2D(warpY, warpX));
        }

        private static float Single(NoiseProfile noiseProfile, WarpProfile warpProfile, float noiseX, float noiseY, float offsetZ, float warpX, float warpY, float warpOffsetZ)
        {
            return Single(
                noiseProfile,
                noiseX - warpProfile.GetWarp3D(warpX, warpY, warpOffsetZ),
                noiseY - warpProfile.GetWarp3D(warpY, warpX, warpOffsetZ),
                offsetZ);
        }

        private static void Noise2D(ref float[] noiseMap, NoiseProfile noiseProfile)
        {
            if (noiseProfile.ComputeRoll)
            {
                float sin = 0, cos = 0;
                CalculateAngles(ref sin, ref cos, noiseProfile.Roll);

                for (int y = 0, i = 0; y < _yNoisePoints.Length; y++)
                {
                    for (int x = 0; x < _xNoisePoints.Length; x++, i++)
                    {
                        noiseMap[i] = Single(
                            noiseProfile,
                            RotateX(_xNoisePoints[x], _yNoisePoints[y], sin, cos),
                            RotateY(_xNoisePoints[x], _yNoisePoints[y], sin, cos)
                        );
                    }
                }
            }
            else
            {
                for (int y = 0, i = 0; y < _yNoisePoints.Length; y++)
                {
                    for (int x = 0; x < _xNoisePoints.Length; x++, i++)
                    {
                        noiseMap[i] = Single(noiseProfile, _xNoisePoints[x], _yNoisePoints[y]);
                    }
                }
            }
        }

        private static void Noise3D(ref float[] noiseMap, NoiseProfile noiseProfile)
        {
            if (noiseProfile.ComputeRoll)
            {
                float sin = 0, cos = 0;
                CalculateAngles(ref sin, ref cos, noiseProfile.Roll);

                for (int y = 0, i = 0; y < _yNoisePoints.Length; y++)
                {
                    for (int x = 0; x < _xNoisePoints.Length; x++, i++)
                    {
                        noiseMap[i] = Single(
                            noiseProfile,
                            RotateX(_xNoisePoints[x], _yNoisePoints[y], sin, cos),
                            RotateY(_xNoisePoints[x], _yNoisePoints[y], sin, cos),
                            noiseProfile.Offset.z
                        );
                    }
                }
            }
            else
            {
                for (int y = 0, i = 0; y < _yNoisePoints.Length; y++)
                {
                    for (int x = 0; x < _xNoisePoints.Length; x++, i++)
                    {
                        noiseMap[i] = Single(noiseProfile, _xNoisePoints[x], _yNoisePoints[y], noiseProfile.Offset.z);
                    }
                }
            }
        }

        private static void WarpedNoise2D(ref float[] noiseMap, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            if (noiseProfile.ComputeRoll)
            {
                float sin = 0, cos = 0;
                CalculateAngles(ref sin, ref cos, noiseProfile.Roll);

                for (int y = 0, i = 0; y < _yNoisePoints.Length; y++)
                {
                    for (int x = 0; x < _xNoisePoints.Length; x++, i++)
                    {
                        noiseMap[i] = Single(
                            noiseProfile,
                            warpProfile,
                            RotateX(_xNoisePoints[x], _yNoisePoints[y], sin, cos),
                            RotateY(_xNoisePoints[x], _yNoisePoints[y], sin, cos),
                            RotateX(_xWarpPoints[x], _yWarpPoints[y], sin, cos),
                            RotateY(_xWarpPoints[x], _yWarpPoints[y], sin, cos)
                        );
                    }
                }
            }
            else
            {
                for (int y = 0, i = 0; y < _yNoisePoints.Length; y++)
                {
                    for (int x = 0; x < _xNoisePoints.Length; x++, i++)
                    {
                        noiseMap[i] = Single(noiseProfile, warpProfile, _xNoisePoints[x], _yNoisePoints[y], _xWarpPoints[x], _yWarpPoints[y]);
                    }
                }
            }
        }

        private static void WarpedNoise3D(ref float[] noiseMap, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            if (noiseProfile.ComputeRoll)
            {
                float sin = 0, cos = 0;
                CalculateAngles(ref sin, ref cos, noiseProfile.Roll);

                for (int y = 0, i = 0; y < _yNoisePoints.Length; y++)
                {
                    for (int x = 0; x < _xNoisePoints.Length; x++, i++)
                    {
                        noiseMap[i] = Single(
                            noiseProfile, 
                            warpProfile,
                            RotateX(_xNoisePoints[x], _yNoisePoints[y], sin, cos),
                            RotateY(_xNoisePoints[x], _yNoisePoints[y], sin, cos), 
                            noiseProfile.Offset.z,
                            RotateX(_xWarpPoints[x], _yWarpPoints[y], sin, cos),
                            RotateY(_xWarpPoints[x], _yWarpPoints[y], sin, cos), 
                            warpProfile.Offset.z
                        );
                    }
                }
            }
            else
            {
                for (int y = 0, i = 0; y < _yNoisePoints.Length; y++)
                {
                    for (int x = 0; x < _xNoisePoints.Length; x++, i++)
                    {
                        noiseMap[i] = Single(noiseProfile, warpProfile, _xNoisePoints[x], _yNoisePoints[y], noiseProfile.Offset.z, _xWarpPoints[x], _yWarpPoints[y], warpProfile.Offset.z);
                    }
                }
            }
        }
    }
}
