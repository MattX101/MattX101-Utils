using Utils.Noise;
using UnityEngine;

namespace Utils.Mask
{
    public static class MaskGenerator
    {        
        public static ComputeShader Shader
        {
            get;
            set;
        }

        private static void Init(int width, int height, MaskSettings settings, ref Vector2 offset, ref Vector2 scale)
        {
            const float DefaultCanvasSize = 1000.0f;
            float canvasToWidthRatio = DefaultCanvasSize / width;
            float canvasToHeightRatio = DefaultCanvasSize / height;

            float canvasToScreenRatio =
                canvasToWidthRatio > canvasToHeightRatio ?
                canvasToWidthRatio :
                canvasToHeightRatio;
            
            scale /= settings.ZoomOut;
            scale *= new Vector2(canvasToScreenRatio, canvasToScreenRatio);

            if (width > height)
            {
                float ratio = (float)width / height;

                scale /= ratio;

                offset.y = (1.0f / ratio - 1.0f) * settings.ZoomOut;
                offset += settings.Offset / ratio;
                offset /= settings.ZoomOut;
            }
            else if (height > width)
            {
                float ratio = (float)height / width;

                scale /= ratio;

                offset.x = (1.0f / ratio - 1.0f) * settings.ZoomOut;
                offset += settings.Offset / ratio;
                offset /= settings.ZoomOut;
            }
            else
            {
                offset += settings.Offset / settings.ZoomOut;
            }

            offset *= canvasToScreenRatio;
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

        public static void Generate(ref float[] values, int width, int height, MaskSettings settings, float canvasToScreenRatio)
        {
            float sin = 0, cos = 0;
            CalculateAngles(ref sin, ref cos, settings.Roll);

            Vector2 offset = Vector2.zero;
            Vector2 scale = settings.Scale;
            Init(width, height, settings, ref offset, ref scale);

            int bound = width > height ? width : height;
            float[] pointsX = GetPoints(bound, offset.x, canvasToScreenRatio);
            float[] pointsY = GetPoints(bound, offset.y, canvasToScreenRatio);

            if (settings.ApplyNoise)
            {
                settings.NoiseProfile.ExternalScale = Vector3.one / settings.ZoomOut;
                settings.WarpProfile.ExternalScale = Vector3.one / settings.ZoomOut;

                const float DefaultCanvasSize = 1000.0f;
                Vector2 noiseOffset = DefaultCanvasSize / 2.0f * settings.Offset;
                Vector3 noiseOffsetToScaleAdjustment = settings.NoiseProfile.Scale * settings.ZoomOut;
                Vector3 warpOffsetToScaleAdjustment = settings.WarpProfile.Scale * settings.ZoomOut;

                settings.NoiseProfile.ExternalOffset = -noiseOffset * settings.NoiseProfile.UniversalScale / noiseOffsetToScaleAdjustment;
                settings.WarpProfile.ExternalOffset = -noiseOffset * settings.WarpProfile.UniversalScale / warpOffsetToScaleAdjustment;
                
                settings.NoiseProfile.Init();
                settings.WarpProfile.Init();

                FastNoise2D.Generate(ref values, width, height, false, settings.NoiseProfile, settings.WarpProfile);

                if (settings.ComputeRoll)
                {
                    for (int y = 0, i = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++, i++)
                        {
                            GetValue_Noise(
                                ref values[i],
                                RotateX(pointsX[x], pointsY[y], sin, cos),
                                RotateY(pointsX[x], pointsY[y], sin, cos),
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
                            GetValue_Noise(ref values[i], pointsX[x], pointsY[y], settings.Warp, scale);
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
                                RotateX(pointsX[x], pointsY[y], sin, cos),
                                RotateY(pointsX[x], pointsY[y], sin, cos),
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
                            values[i] = GetValue(pointsX[x], pointsY[y], scale);
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

        public static void Generate(ref ComputeBuffer buffer, int width, int height, MaskSettings settings, float canvasToScreenRatio)
        {
            float sin = 0, cos = 0;
            CalculateAngles(ref sin, ref cos, settings.Roll);

            Vector2 offset = Vector2.zero;
            Vector2 scale = settings.Scale;
            Init(width, height, settings, ref offset, ref scale);

            int bound = width > height ? width : height;
            ComputeBuffer pointsXBuffer = new ComputeBuffer(bound, sizeof(float));
            ComputeBuffer pointsYBuffer = new ComputeBuffer(bound, sizeof(float));
            pointsXBuffer.SetData(GetPoints(bound, offset.x, canvasToScreenRatio));
            pointsYBuffer.SetData(GetPoints(bound, offset.y, canvasToScreenRatio));

            Shader.SetFloat(UnityEngine.Shader.PropertyToID("sin"), sin);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("cos"), cos);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("width"), width);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("height"), height);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("warp"), settings.Warp);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("power"), settings.Power);
            Shader.SetFloat(UnityEngine.Shader.PropertyToID("ratio"), canvasToScreenRatio);
            
            Shader.SetFloats(UnityEngine.Shader.PropertyToID("scale"), scale.x, scale.y);
            Shader.SetFloats(UnityEngine.Shader.PropertyToID("range"), settings.MinValue, settings.MaxValue);

            int kernel;
            if (settings.ApplyNoise)
            {
                settings.NoiseProfile.ExternalScale = Vector3.one / settings.ZoomOut;
                settings.WarpProfile.ExternalScale = Vector3.one / settings.ZoomOut;
                
                const float DefaultCanvasSize = 1000.0f;
                Vector2 noiseOffset = DefaultCanvasSize / 2.0f * settings.Offset;
                Vector3 noiseOffsetToScaleAdjustment = settings.NoiseProfile.Scale * settings.ZoomOut;
                Vector3 warpOffsetToScaleAdjustment = settings.WarpProfile.Scale * settings.ZoomOut;

                settings.NoiseProfile.ExternalOffset = -noiseOffset * settings.NoiseProfile.UniversalScale / noiseOffsetToScaleAdjustment;
                settings.WarpProfile.ExternalOffset = -noiseOffset * settings.WarpProfile.UniversalScale / warpOffsetToScaleAdjustment;
                
                settings.NoiseProfile.Init();
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
                Mathf.Abs(Mathf.Pow(x / scale.y, 2)) + 
                Mathf.Abs(Mathf.Pow(y / scale.x, 2))
            );
        }

        private static void GetValue_Noise(ref float value, float x, float y, float warp, Vector2 scale)
        {
            value = GetValue(x, y, scale) + (value * warp);
            value = Mathf.Clamp(value, 0, warp + 1);
            value = Mathf.InverseLerp(0, warp + 1, value);
        }

        private static float RotateX(float x, float y, float sin, float cos) => x * sin + y * cos;
        private static float RotateY(float x, float y, float sin, float cos) => x * cos - y * sin;

        private static void CalculateAngles(ref float sin, ref float cos, float roll)
        {
            sin = Mathf.Sin(roll * Mathf.Deg2Rad);
            cos = Mathf.Cos(roll * Mathf.Deg2Rad);
        }
    }
}
