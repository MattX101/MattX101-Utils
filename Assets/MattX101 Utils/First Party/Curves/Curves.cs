using UnityEngine;

namespace Utils.Curves
{
    public static partial class Curves
    {
        public static ComputeShader Shader
        {
            get;
            set;
        }

        public static ComputeShader AnimationCurveShader
        {
            get;
            set;
        }


        private const int InterpolationCurvePoints = 32;

        public enum Equations
        {
            EaseIn,
            EaseInCirc,
            EaseInOut,
            EaseInOutSine,
            Sine,
            SineSqrt,
            RepeatedSine,
            HalfDownUpSine,
            SineFrequency,
            RadianArcSine,
            RadianArcSineSqrt
        };

        private delegate float Equation(float v, float p);

        public static void SetToCurve(ref float value, Equations mode, float power)
        {
            Equation equation = GetEquation(mode);
            value = equation(value, power);
        }

        public static void SetToCurve(ref float[] values, Equations mode, float power)
        {
            Equation equation = GetEquation(mode);

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = equation(values[i], power);
            }
        }

        public static void SetToCurve(ref Color[] values, Equations mode, float power)
        {
            Equation equation = GetEquation(mode);

            for (int i = 0; i < values.Length; i++)
            {
                values[i].r = equation(values[i].r, power);
                values[i].g = equation(values[i].g, power);
                values[i].b = equation(values[i].b, power);
            }
        }

        public static void SetToCurve(ref ComputeBuffer valuesBuffer, Equations mode, float power, bool isFloat)
        {
            int kernel = 0;
            if (isFloat)
            {
                kernel = Shader.FindKernel(GetEquationAsString(mode) + "_Single");
                Shader.SetBuffer(kernel, "values", valuesBuffer);
            }
            else
            {
                kernel = Shader.FindKernel(GetEquationAsString(mode) + "_Image");
                Shader.SetBuffer(kernel, "image", valuesBuffer);
            }

            Shader.SetFloat("power", power);
            Shader.Dispatch(kernel, Mathf.CeilToInt(valuesBuffer.count / 1024.0f), 1, 1);
        }

        private static Equation GetEquation(Equations equation)
        {
            return equation switch
            {
                Equations.EaseIn => CurveFormules.EaseIn,
                Equations.EaseInCirc => CurveFormules.EaseInCirc,
                Equations.EaseInOut => CurveFormules.EaseInOut,
                Equations.EaseInOutSine => CurveFormules.EaseInOutSine,
                Equations.Sine => CurveFormules.Sine,
                Equations.SineSqrt => CurveFormules.SineSqrt,
                Equations.RepeatedSine => CurveFormules.RepeatedSine,
                Equations.HalfDownUpSine => CurveFormules.HalfDownUpSine,
                Equations.SineFrequency => CurveFormules.SineFrequency,
                Equations.RadianArcSine => CurveFormules.RadianArcSine,
                Equations.RadianArcSineSqrt => CurveFormules.RadianArcSineSqrt,
                _ => CurveFormules.EaseIn
            };
        }

        private static string GetEquationAsString(Equations equation)
        {
            return equation switch
            {
                Equations.EaseIn => "EaseIn",
                Equations.EaseInCirc => "EaseInCirc",
                Equations.EaseInOut => "EaseInOut",
                Equations.EaseInOutSine => "EaseInOutSine",
                Equations.Sine => "Sine",
                Equations.SineSqrt => "SineSqrt",
                Equations.RepeatedSine => "RepeatedSine",
                Equations.HalfDownUpSine => "HalfDownUpSine",
                Equations.SineFrequency => "SineFrequency",
                Equations.RadianArcSine => "RadianArcSine",
                Equations.RadianArcSineSqrt => "RadianArcSineSqrt",
                _ => "EaseIn"
            };
        }

        // ------------ //
        // Custom Curve //
        // ------------ //

        public static void SetToCurve(ref float value, AnimationCurve curve)
        {
            value = curve.Evaluate(value);
        }

        public static void SetToCurve(ref Color color, AnimationCurve curve)
        {
            color.r = curve.Evaluate(color.r);
            color.g = curve.Evaluate(color.g);
            color.b = curve.Evaluate(color.b);
        }

        public static void SetToCurve(ref float[] values, AnimationCurve curve)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = curve.Evaluate(values[i]);
            }
        }

        public static void SetToCurve(ref Color[] colors, AnimationCurve curve)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i].r = curve.Evaluate(colors[i].r);
                colors[i].g = curve.Evaluate(colors[i].g);
                colors[i].b = curve.Evaluate(colors[i].b);
            }
        }

        public static void SetToCurve(ComputeBuffer valuesBuffer, AnimationCurve curve, bool isFloat)
        {
            ComputeBuffer curveBuffer = new ComputeBuffer(InterpolationCurvePoints, sizeof(float) * 2);
            curveBuffer.SetData(InterpolateCurve(curve));

            int kernel = 0;
            if (isFloat)
            {
                kernel = AnimationCurveShader.FindKernel("SetToCurveFloat");
                AnimationCurveShader.SetBuffer(kernel, "noise", valuesBuffer);
            }
            else
            {
                kernel = AnimationCurveShader.FindKernel("SetToCurveColor");
                AnimationCurveShader.SetBuffer(kernel, "color", valuesBuffer);
            }

            AnimationCurveShader.SetBuffer(kernel, "curve", curveBuffer);
            AnimationCurveShader.SetInt("length", InterpolationCurvePoints - 1);

            AnimationCurveShader.Dispatch(kernel, Mathf.CeilToInt((float)valuesBuffer.count / 1024.0f), 1, 1);

            curveBuffer.Release();
        }

        private static Vector2[] InterpolateCurve(AnimationCurve curve)
        {
            Vector2[] interpolation = new Vector2[InterpolationCurvePoints];
            float time = 0.0f;

            for (int i = 0; i < InterpolationCurvePoints; i++)
            {
                time = (float)i / (InterpolationCurvePoints - 1);

                interpolation[i].x = curve.Evaluate(time);
                interpolation[i].y = time;
            }

            return interpolation;
        }
    }
}
