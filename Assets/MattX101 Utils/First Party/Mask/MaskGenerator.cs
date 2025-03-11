using Utils.Noise;
using UnityEngine;

namespace Utils.Mask
{
    public static class MaskGenerator
    {
        private static float _sin;
        private static float _cos;

        private static float _canvasToWidthRatio;
        private static float _canvasToHeightRatio;
        private static float _canvasRatio;

        private static float GetStart(float offset, float ratio)
        {
            return -ratio - offset;
        }

        public static void Generate(ref float[] values, int width, int height, MaskSettings settings)
        {
            _canvasToWidthRatio = width / 1000.0f;
            _canvasToHeightRatio = height / 1000.0f;
            _canvasRatio = _canvasToWidthRatio < _canvasToHeightRatio ? _canvasToWidthRatio : _canvasToHeightRatio;

            if (settings.ApplyNoise)
            {
                settings.NoiseProfile.Init();
                settings.WarpProfile.Init();

                FastNoise2D.Generate(ref values, width, height, false, settings.NoiseProfile, settings.WarpProfile);

                for (int i = 0; i < values.Length; i++)
                {
                    values[i] *= settings.Warp * _canvasRatio;
                }
            }

            CalculateAngles(-settings.Roll);

            float xP = GetStart(settings.Offset.x, _canvasToWidthRatio);
            float yP = GetStart(settings.Offset.y, _canvasToHeightRatio);

            float xInc = (_canvasToWidthRatio * 2.0f) / width;
            float yInc = (_canvasToHeightRatio * 2.0f) / height; 

            Vector2 correctedScale = settings.Scale / _canvasRatio;

            if (settings.ApplyNoise)
            {
                for (int y = 0, i = 0; y < height; y++, yP += yInc)
                {
                    for (int x = 0; x < width; x++, xP += xInc, i++)
                    {
                        GetValue_Noise(
                            ref values[i],
                            RotateX(xP, yP),
                            RotateY(xP, yP), 
                            correctedScale
                        );
                    }

                    xP = GetStart(settings.Offset.x, _canvasToWidthRatio);
                }
            }
            else
            {
                for (int y = 0, i = 0; y < height; y++, yP += yInc)
                {
                    for (int x = 0; x < width; x++, xP += xInc, i++)
                    {
                        GetValue(
                            ref values[i],
                            RotateX(xP, yP),
                            RotateY(xP, yP), 
                            correctedScale
                        );
                    }

                    xP = GetStart(settings.Offset.x, _canvasToWidthRatio);
                }
            }

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = Mathf.InverseLerp(0, settings.MaxValue, values[i]);
                values[i] = Mathf.Pow(values[i], Mathf.Max(Mathf.Abs(settings.Power), 1));
            }

            if (settings.Invert)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = 1 - values[i];
                }
            }

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = Mathf.Clamp(values[i], settings.MinValue, 1);
            }
        }

        private static void GetValue(ref float value, float x, float y, Vector2 scale)
        {
            value = Mathf.Sqrt(
                Mathf.Abs(Mathf.Pow(x * scale.x, 2)) + 
                Mathf.Abs(Mathf.Pow(y * scale.y, 2))
            );
        }

        private static void GetValue_Noise(ref float value, float x, float y, Vector2 scale)
        {
            x = x < 0 ? x + value : x - value;
            y = y < 0 ? y + value : y - value;

            GetValue(ref value, x, y, scale);
        }

        private static float RotateX(float x, float y) => x * _sin + y * _cos;
        private static float RotateY(float x, float y) => x * _cos - y * _sin;

        private static void CalculateAngles(float roll)
        {
            _sin = Mathf.Cos(roll * Mathf.Deg2Rad);
            _cos = Mathf.Sin(roll * Mathf.Deg2Rad);
        }
    }
}
