using Utils.Noise;
using UnityEngine;

namespace Utils.Mask
{
    public static class MaskGenerator
    {
        private static float _sin, _cos;

        private static float _canvasToWidthRatio;
        private static float _canvasToHeightRatio;
        private static float _canvasRatio;

        public static ComputeShader Shader
        {
            get;
            set;
        }

        private static float GetStart(float offset, float ratio)
        {
            return -ratio - offset;
        }

        private static void Init(int width, int height, MaskSettings settings)
        {
            _canvasToWidthRatio = width / 1000.0f;
            _canvasToHeightRatio = height / 1000.0f;
            _canvasRatio = _canvasToWidthRatio < _canvasToHeightRatio ? _canvasToWidthRatio : _canvasToHeightRatio;

            CalculateAngles(-settings.Roll);
        }

        private static float[] GetPoints(int res, float offset, float ratio)
        {
            float[] points = new float[res];

            float pos = GetStart(offset, ratio);
            float inc = (ratio * 2.0f) / res;

            for (int i = 0; i < res; i++)
            {
                points[i] = pos;
                pos += inc;
            }

            return points;
        }

        public static void Generate(ref float[] values, int width, int height, MaskSettings settings)
        {
            Init(width, height, settings);

            Vector2 scale = settings.Scale / _canvasRatio;
            
            if (settings.ApplyNoise)
            {
                settings.NoiseProfile.Init();
                settings.WarpProfile.Init();

                FastNoise2D.Generate(ref values, width, height, false, settings.NoiseProfile, settings.WarpProfile);
            }

            float[] pointsX = GetPoints(width, settings.Offset.x, _canvasToWidthRatio);
            float[] pointsY = GetPoints(height, settings.Offset.y, _canvasToHeightRatio);

            if (settings.ApplyNoise)
            {
                for (int y = 0, i = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++, i++)
                    {
                        GetValue_Noise(
                            ref values[i],
                            RotateX(pointsX[x], pointsY[y]),
                            RotateY(pointsX[x], pointsY[y]),
                            settings.Warp,
                            scale
                        );
                    }
                }
            }
            else
            {
                for (int y = 0, i = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++, i++)
                    {
                        GetValue(
                            ref values[i],
                            RotateX(pointsX[x], pointsY[y]),
                            RotateY(pointsX[x], pointsY[y]),
                            scale
                        );
                    }
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

        public static void Generate(ref ComputeBuffer buffer, int width, int height, MaskSettings settings)
        {
            Init(width, height, settings);

            ComputeBuffer pointsXBuffer = new ComputeBuffer(width, sizeof(float));
            pointsXBuffer.SetData(GetPoints(width, settings.Offset.x, _canvasToWidthRatio));

            ComputeBuffer pointsYBuffer = new ComputeBuffer(height, sizeof(float));
            pointsYBuffer.SetData(GetPoints(height, settings.Offset.y, _canvasToHeightRatio));

            Shader.SetFloat("sin", _sin);
            Shader.SetFloat("cos", _cos);

            Shader.SetFloat("width", width);
            Shader.SetFloat("height", height);

            Shader.SetFloat("power", settings.Power);
            
            Shader.SetFloats(
                "scale", 
                settings.Scale.x / _canvasRatio, 
                settings.Scale.y / _canvasRatio
            );
            Shader.SetFloats(
                "range", 
                settings.MinValue, 
                settings.MaxValue
            );

            Shader.SetFloat("warp", settings.Warp);
            Shader.SetFloat("ratio", _canvasRatio);

            int kernel = 0;
            if (settings.ApplyNoise)
            {
                FastNoise2D.Generate(ref buffer, width, height, false, settings.NoiseProfile, settings.WarpProfile);

                kernel = settings.Invert ? Shader.FindKernel("Noise_Invert") : Shader.FindKernel("Noise");
            }
            else
            {
                kernel = settings.Invert ? Shader.FindKernel("Normal_Invert") : Shader.FindKernel("Normal");
            }

            Shader.SetBuffer(kernel, "pointsX", pointsXBuffer);
            Shader.SetBuffer(kernel, "pointsY", pointsYBuffer);
            Shader.SetBuffer(kernel, "values", buffer);

            Shader.Dispatch(kernel, Mathf.CeilToInt(width / 32.0f), Mathf.CeilToInt(height / 32.0f), 1);

            pointsXBuffer.Dispose();
            pointsYBuffer.Dispose();
        }

        private static void GetValue(ref float value, float x, float y, Vector2 scale)
        {
            value = Mathf.Sqrt(
                Mathf.Abs(Mathf.Pow(x * scale.x, 2)) + 
                Mathf.Abs(Mathf.Pow(y * scale.y, 2))
            );
        }

        private static void GetValue_Noise(ref float value, float x, float y, float warp, Vector2 scale)
        {
            value *= warp * _canvasRatio;

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
