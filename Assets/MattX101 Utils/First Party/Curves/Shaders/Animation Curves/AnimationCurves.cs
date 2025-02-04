using UnityEngine;

namespace Utils.Curves
{
    public static class AnimationCurves
    {
        public static ComputeShader Shader
        {
            get;
            set;
        }

        private const int InterpolationCurvePoints = 32;

        public static void SetToCurveFloatCPU(ref float[] noiseMap, AnimationCurve curve)
        {
            for (int i = 0; i < noiseMap.Length; i++)
            {
                noiseMap[i] = curve.Evaluate(noiseMap[i]);
            }
        }

        public static void SetToCurveFloatGPU(ComputeBuffer noiseBuffer, AnimationCurve curve, int length)
        {
            Vector2[] interpolation = InterpolateCurve(curve);

            ComputeBuffer curveBuffer = new ComputeBuffer(interpolation.Length, sizeof(float) * 2);
            curveBuffer.SetData(interpolation);

            int kernel = Shader.FindKernel("SetToCurveFloat");
            Shader.SetBuffer(kernel, "curve", curveBuffer);
            Shader.SetBuffer(kernel, "noise", noiseBuffer);

            Shader.SetInt("length", interpolation.Length - 1);

            Shader.Dispatch(
                kernel,
                Mathf.CeilToInt((float)length / 1024.0f),
                1,
                1);

            curveBuffer.Release();
        }

        public static void SetToCurveColorCPU(ref Color[] colourMap, AnimationCurve curve)
        {
            for (int i = 0; i < colourMap.Length; i++)
            {
                colourMap[i] = new Color(
                    curve.Evaluate(colourMap[i].r),
                    curve.Evaluate(colourMap[i].g),
                    curve.Evaluate(colourMap[i].b));
            }
        }

        public static void SetToCurveColorGPU(ComputeBuffer colourBuffer, AnimationCurve curve, int length)
        {
            Vector2[] interpolation = InterpolateCurve(curve);

            ComputeBuffer curveBuffer = new ComputeBuffer(interpolation.Length, sizeof(float) * 2);
            curveBuffer.SetData(interpolation);

            int kernel = Shader.FindKernel("SetToCurveColor");
            Shader.SetBuffer(kernel, "curve", curveBuffer);
            Shader.SetBuffer(kernel, "color", colourBuffer);

            Shader.SetInt("length", interpolation.Length - 1);

            Shader.Dispatch(
                kernel,
                Mathf.CeilToInt((float)length / 1024.0f),
                1,
                1);

            curveBuffer.Release();
        }

        private static Vector2[] InterpolateCurve(AnimationCurve curve)
        {
            Vector2[] interpolation = new Vector2[InterpolationCurvePoints];

            for (int i = 0; i < interpolation.Length; i++)
            {
                float time = (float)i / (interpolation.Length - 1);

                interpolation[i].x = curve.Evaluate(time);
                interpolation[i].y = time;
            }

            return interpolation;
        }
    }
}
