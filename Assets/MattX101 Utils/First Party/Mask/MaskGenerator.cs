using Utils.Noise;
using UnityEngine;

namespace Utils.Mask
{
    public static class MaskGenerator
    {
        private const float DefaultCanvasSize = 1000.0f;

        private static float CanvasToScreenRatio { get; set; } = 1.0f;

        private static float CanvasToWidthRatio { get; set; }
        private static float CanvasToHeightRatio { get; set; }

        private static float _sin, _cos;

        private static int _bound;

        private static Vector2 _offset, _scale;
        
        public static ComputeShader Shader
        {
            get;
            set;
        }

        private static void Init(int width, int height, MaskSettings settings)
        {
            CanvasToWidthRatio = DefaultCanvasSize / width;
            CanvasToHeightRatio = DefaultCanvasSize / height;

            CanvasToScreenRatio =
                CanvasToWidthRatio > CanvasToHeightRatio ?
                CanvasToWidthRatio :
                CanvasToHeightRatio;

            CalculateAngles(-settings.Roll);
            
            _offset = Vector2.zero;

            _scale = settings.Scale;
            _scale /= settings.Zoom;
            _scale /= new Vector2(CanvasToScreenRatio, CanvasToScreenRatio);

            if (width > height)
            {
                float ratio = (float)width / height;

                _scale *= ratio;

                _offset.y = (1.0f / ratio - 1.0f) / settings.Zoom;
                _offset += settings.Offset / ratio;
                _offset *= settings.Zoom;
            }
            else if (height > width)
            {
                float ratio = (float)height / width;

                _scale *= ratio;

                _offset.x = (1.0f / ratio - 1.0f) / settings.Zoom;
                _offset += settings.Offset / ratio;
                _offset *= settings.Zoom;
            }
            else
            {
                _offset += settings.Offset * settings.Zoom;
            }

            _offset *= CanvasToScreenRatio;

            _bound = width > height ? width : height;
        }

        private static float[] GetPoints(int res, float offset, float ratio)
        {
            float[] points = new float[res];

            float pos = -ratio - offset;
            float inc = (ratio * 2.0f) / res;

            pos += (inc / 2.0f);

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

            float[] pointsX = GetPoints(_bound, _offset.x, CanvasToScreenRatio);
            float[] pointsY = GetPoints(_bound, _offset.y, CanvasToScreenRatio);

            if (settings.ApplyNoise)
            {
                Vector2 noiseOffset = DefaultCanvasSize / 2.0f * settings.Offset;

                settings.NoiseProfile.SetScale(Vector3.one * settings.Zoom);
                settings.WarpProfile.SetScale(Vector3.one * settings.Zoom);

                settings.NoiseProfile.SetOffset(noiseOffset);
                settings.WarpProfile.SetOffset(noiseOffset);

                settings.NoiseProfile.Init();
                settings.WarpProfile.Init();

                FastNoise2D.Generate(ref values, width, height, false, settings.NoiseProfile, settings.WarpProfile);

                if (settings.PreviewNoise)
                    return;

                if (settings.ComputeRoll)
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
                                _scale
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
                            GetValue_Noise(ref values[i], pointsX[x], pointsY[y], settings.Warp, _scale);
                        }
                    }
                }
            }
            else
            {
                if (settings.ComputeRoll)
                {
                    for (int y = 0, i = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++, i++)
                        {
                            values[i] = GetValue(
                                RotateX(pointsX[x], pointsY[y]),
                                RotateY(pointsX[x], pointsY[y]),
                                _scale
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
                            values[i] = GetValue(pointsX[x], pointsY[y], _scale);
                        }
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
            ComputeBuffer pointsYBuffer = new ComputeBuffer(height, sizeof(float));
            pointsXBuffer.SetData(GetPoints(_bound, _offset.x, CanvasToScreenRatio));
            pointsYBuffer.SetData(GetPoints(_bound, _offset.y, CanvasToScreenRatio));

            Shader.SetFloat(UnityEngine.Shader.PropertyToID("sin"), _sin);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("cos"), _cos);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("width"), width);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("height"), height);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("warp"), settings.Warp);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("power"), settings.Power);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("ratio"), CanvasToScreenRatio);
            
            Shader.SetFloats(UnityEngine.Shader.PropertyToID("scale"), _scale.x, _scale.y);
            Shader.SetFloats(UnityEngine.Shader.PropertyToID("range"), settings.MinValue, settings.MaxValue);

            int kernel = 0;
            if (settings.ApplyNoise)
            {
                Vector2 noiseOffset = DefaultCanvasSize / 2.0f * settings.Offset;

                settings.NoiseProfile.SetScale(Vector3.one * settings.Zoom);
                settings.NoiseProfile.SetOffset(noiseOffset);
                settings.NoiseProfile.Init();
                
                settings.WarpProfile.SetScale(Vector3.one * settings.Zoom);
                settings.WarpProfile.SetOffset(noiseOffset);
                settings.WarpProfile.Init();

                FastNoise2D.Generate(ref buffer, width, height, false, settings.NoiseProfile, settings.WarpProfile);
                
                kernel = 
                    settings.Invert ? 
                    Shader.FindKernel(settings.ComputeRoll ? "Noise_Invert_Roll" : "Noise_Invert") : 
                    Shader.FindKernel(settings.ComputeRoll ? "Noise_Roll" : "Noise");
            }
            else
            {
                kernel = 
                    settings.Invert ? 
                    Shader.FindKernel(settings.ComputeRoll ? "Normal_Invert_Roll" : "Normal_Invert") : 
                    Shader.FindKernel(settings.ComputeRoll ? "Norma_Roll" : "Normal");
            }

            Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("values"), buffer);
            Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("pointsX"), pointsXBuffer);
            Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("pointsY"), pointsYBuffer);

            Shader.Dispatch(kernel, Mathf.CeilToInt(width / 32.0f), Mathf.CeilToInt(height / 32.0f), 1);

            pointsXBuffer.Dispose();
            pointsYBuffer.Dispose();
        }

        private static float GetValue(float x, float y, Vector2 scale)
        {
            return Mathf.Sqrt(
                Mathf.Abs(Mathf.Pow(x * scale.x, 2)) + 
                Mathf.Abs(Mathf.Pow(y * scale.y, 2))
            );
        }

        private static void GetValue_Noise(ref float value, float x, float y, float warp, Vector2 scale)
        {
            value = GetValue(x, y, scale) + (value * warp);
            value = Mathf.Clamp(value, 0, warp + 1);
            value = Mathf.InverseLerp(0, warp + 1, value);
        }

        private static float RotateX(float x, float y) => x * _sin + y * _cos;
        private static float RotateY(float x, float y) => x * _cos - y * _sin;

        private static void CalculateAngles(float roll)
        {
            _sin = Mathf.Sin(roll * Mathf.Deg2Rad);
            _cos = Mathf.Cos(roll * Mathf.Deg2Rad);
        }
    }
}
