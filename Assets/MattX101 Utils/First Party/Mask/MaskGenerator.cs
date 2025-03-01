using UnityEngine;

namespace Utils.Mask
{
    public static class MaskGenerator
    {
        private static float _sin;
        private static float _cos;

        private static float GetStart(float offset, float ratio)
        {
            return -ratio - offset;
        }

        public static void Generate(ref float[] values, int width, int height, MaskSettings settings)
        {
            CalculateAngles(-settings.Roll);

            float canvasToWidthRatio = width / 1000.0f;
            float canvasToHeightRatio = height / 1000.0f;

            float xInc = (canvasToWidthRatio * 2.0f) / width;
            float yInc = (canvasToHeightRatio * 2.0f) / height;

            float xP = GetStart(settings.Offset.x, canvasToWidthRatio);
            float yP = GetStart(settings.Offset.y, canvasToHeightRatio);

            Vector2 correctedScale = settings.Sclae / (canvasToWidthRatio < canvasToHeightRatio ? canvasToWidthRatio : canvasToHeightRatio);

            if (settings.Invert)
            {
                for (int y = 0, i = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++, i++)
                    {
                        values[i] = CalcualteInvertedMask(xP, yP, correctedScale, settings.Power, settings.MinValue, settings.MaxValue);

                        xP += xInc;
                    }

                    xP = GetStart(settings.Offset.x, canvasToWidthRatio);
                    yP += yInc;
                }
            }
            else
            {
                for (int y = 0, i = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++, i++)
                    {
                        values[i] = CalcualteMask(xP, yP, correctedScale, settings.Power, settings.MinValue, settings.MaxValue);

                        xP += xInc;
                    }

                    xP = GetStart(settings.Offset.x, canvasToWidthRatio);
                    yP += yInc;
                }
            }
        }

        public static void Generate(ref Color[] colors, int width, int height, MaskSettings settings)
        {
            CalculateAngles(-settings.Roll);

            float canvasToWidthRatio = width / 1000.0f;
            float canvasToHeightRatio = height / 1000.0f;

            float xInc = (canvasToWidthRatio * 2.0f) / width;
            float yInc = (canvasToHeightRatio * 2.0f) / height;

            float startX = -canvasToWidthRatio - settings.Offset.x;
            float startY = -canvasToHeightRatio - settings.Offset.y;

            float xP = startX;
            float yP = startY;

            Vector2 correctedScale = settings.Sclae / (canvasToWidthRatio < canvasToHeightRatio ? canvasToWidthRatio : canvasToHeightRatio);

            if (settings.Invert)
            {
                for (int y = 0, i = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++, i++)
                    {
                        colors[i] = Color.Lerp(Color.black, Color.white, CalcualteInvertedMask(xP, yP, correctedScale, settings.Power, settings.MinValue, settings.MaxValue));
                        colors[i].a = 1;

                        xP += xInc;
                    }

                    xP = startX;
                    yP += yInc;
                }
            }
            else
            {
                for (int y = 0, i = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++, i++)
                    {
                        colors[i] = Color.Lerp(Color.black, Color.white, CalcualteMask(xP, yP, correctedScale, settings.Power, settings.MinValue, settings.MaxValue));
                        colors[i].a = 1;

                        xP += xInc;
                    }

                    xP = startX;
                    yP += yInc;
                }
            }
        }

        private static float GetValue(float x, float y, Vector2 scale)
        {
            return Mathf.Sqrt(Mathf.Abs(Mathf.Pow(GetPointX(x, y) * scale.x, 2)) + Mathf.Abs(Mathf.Pow(GetPointY(x, y) * scale.y, 2)));
        }

        private static float CalcualteInvertedMask(float x, float y, Vector2 scale, float power, float min, float max)
        {
            float value = GetValue(x, y, scale);
            value = Mathf.InverseLerp(0, max, value);
            value = 1 - Mathf.Pow(value, Mathf.Max(Mathf.Abs(power), 1));
            value = value < min ? min : value;

            return value;
        }

        private static float CalcualteMask(float x, float y, Vector2 scale, float power, float min, float max)
        {
            float value = GetValue(x, y, scale);
            value = Mathf.InverseLerp(0, max, value);
            value = Mathf.Pow(value, Mathf.Max(Mathf.Abs(power), 1));
            value = value < min ? min : value;

            return value;
        }

        private static void CalculateAngles(float roll)
        {
            _sin = Mathf.Cos(roll * Mathf.Deg2Rad);
            _cos = Mathf.Sin(roll * Mathf.Deg2Rad);
        }

        private static float GetPointX(float x, float y)
        {
            float rotatedX = x * _sin + y * _cos;
            return rotatedX < 0 ? rotatedX : rotatedX;
        }

        private static float GetPointY(float x, float y)
        {
            float rotatedY = x * _cos - y * _sin;
            return rotatedY < 0 ? rotatedY : rotatedY;
        }
    }
}
