using Utils.Noise.Profiles;

namespace Utils.Noise
{
    public class FastNoise2D
    {
        private const float DefaultCanvasSize = 1000.0f;

        public void Noise2DToMap2D(ref float[] noiseMap, int resX, int resY, NoiseProfile noiseProfile)
        {
            GenerateNoise2D(ref noiseMap, resX, resY, noiseProfile, null, false, false);
        }

        public void Noise3DToMap2D(ref float[] noiseMap, int resX, int resY, NoiseProfile noiseProfile)
        {
            GenerateNoise2D(ref noiseMap, resX, resY, noiseProfile, null, true, false);
        }

        public void WarpedNoise2DToMap2D(ref float[] noiseMap, int resX, int resY, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            GenerateNoise2D(ref noiseMap, resX, resY, noiseProfile, warpProfile, false, true);
        }

        public void WarpedNoise3DToMap2D(ref float[] noiseMap, int resX, int resY, NoiseProfile noiseProfile, WarpProfile warpProfile)
        {
            GenerateNoise2D(ref noiseMap, resX, resY, noiseProfile, warpProfile, true, true);
        }

        private void GenerateNoise2D(ref float[] noiseMap, int resX, int resY, NoiseProfile noiseProfile, WarpProfile warpProfile, bool is3D, bool warp)
        {
            float resToLengthRatio = DefaultCanvasSize / resX;
            float resToWidthRatio = DefaultCanvasSize / resY;

            float halfResX = resX / 2.0f;
            float halfResY = resY / 2.0f;

            float offsetX = noiseProfile.offset.x / noiseProfile.scale.x / noiseProfile.universalScale;
            float offsetY = noiseProfile.offset.y / noiseProfile.scale.y / noiseProfile.universalScale;

            float startX = -halfResX + offsetX;
            startX *= noiseProfile.scale.x * noiseProfile.universalScale;
            startX *= resToLengthRatio;
            float startY = -halfResY + offsetY;
            startY *= noiseProfile.scale.y * noiseProfile.universalScale;
            startY *= resToWidthRatio;

            float xInc = noiseProfile.scale.x * noiseProfile.universalScale * resToLengthRatio;
            float yInc = noiseProfile.scale.y * noiseProfile.universalScale * resToWidthRatio;

            float xPos = startX;
            float yPos = startY;

            if (!warp)
            {
                if (is3D)
                {
                    float z = noiseProfile.offset.z * noiseProfile.scale.z * noiseProfile.universalScale;

                    for (int y = 0, i = 0; y < resY; y++)
                    {
                        for (int x = 0; x < resX; x++, i++)
                        {
                            noiseMap[i] = noiseProfile.GetNoise3D(xPos, yPos, z);
                            xPos += xInc;
                        }

                        xPos = startX;
                        yPos += yInc;
                    }
                }
                else
                {
                    for (int y = 0, i = 0; y < resY; y++)
                    {
                        for (int x = 0; x < resX; x++, i++)
                        {
                            noiseMap[i] = noiseProfile.GetNoise2D(xPos, yPos);
                            xPos += xInc;
                        }

                        xPos = startX;
                        yPos += yInc;
                    }
                }
            }
            else if (warp)
            {
                if (warpProfile == null)
                    return;

                float warpOffsetX = warpProfile.offset.x / warpProfile.scale.x / warpProfile.universalScale;
                float warpOffsetY = warpProfile.offset.y / warpProfile.scale.y / warpProfile.universalScale;

                float warpStartX = -halfResX + offsetX + warpOffsetX;
                warpStartX *= noiseProfile.scale.x * noiseProfile.universalScale;
                warpStartX *= resToLengthRatio;
                float warpStartY = -halfResY + offsetY + warpOffsetY;
                warpStartY *= noiseProfile.scale.y * noiseProfile.universalScale;
                warpStartY *= resToWidthRatio;

                float warpScaleX = warpProfile.scale.x * warpProfile.universalScale;
                float warpScaleY = warpProfile.scale.y * warpProfile.universalScale;

                float warpXPos = warpStartX;
                float warpYPos = warpStartY;

                float z = noiseProfile.offset.z * noiseProfile.scale.z * noiseProfile.universalScale;

                if (is3D)
                {
                    for (int y = 0, i = 0; y < resY; y++)
                    {
                        float yy = warpYPos * warpScaleY;
                        for (int x = 0; x < resX; x++, i++)
                        {
                            float xx = warpXPos * warpScaleX;

                            noiseMap[i] = noiseProfile.GetNoise3D(
                                xPos + warpProfile.GetWarp3D(xx, yy, z),
                                yPos + warpProfile.GetWarp3D(yy, xx, -z),
                                z);

                            xPos += xInc;
                            warpXPos += xInc;
                        }

                        xPos = startX;
                        warpXPos = warpStartX;
                        yPos += yInc;
                        warpYPos += yInc;
                    }
                }
                else
                {
                    for (int y = 0, i = 0; y < resY; y++)
                    {
                        float yy = warpYPos * warpScaleY;
                        for (int x = 0; x < resX; x++, i++)
                        {
                            float xx = warpXPos * warpScaleX;

                            noiseMap[i] = noiseProfile.GetNoise2D(
                                xPos + warpProfile.GetWarp2D(xx, yy),
                                yPos + warpProfile.GetWarp2D(yy, xx));

                            xPos += xInc;
                            warpXPos += xInc;
                        }

                        xPos = startX;
                        warpXPos = warpStartX;
                        yPos += yInc;
                        warpYPos += yInc;
                    }
                }
            }
        }
    }
}
