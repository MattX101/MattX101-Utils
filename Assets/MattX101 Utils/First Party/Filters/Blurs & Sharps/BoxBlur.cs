using UnityEngine;

namespace Utils.Filters.Blur
{
    public static class BoxBlur
    {
        public static ComputeShader Shader
        {
            get;
            set;
        }

        private static void Blur(ref float a, float n1, float n2, float n3, float n4, float n5, float n6, float n7, float n8, float n9)
        {
            a = (n1 + n2 + n3 + n4 + n5 + n6 + n7 + n8 + n9) / 9.0f;
        }

        private static void Blur(ref Color a, Color n1, Color n2, Color n3, Color n4, Color n5, Color n6, Color n7, Color n8, Color n9)
        {
            Blur(ref a.r, n1.r, n2.r, n3.r, n4.r, n5.r, n6.r, n7.r, n8.r, n9.r);
            Blur(ref a.g, n1.g, n2.g, n3.g, n4.g, n5.g, n6.g, n7.g, n8.g, n9.g);
            Blur(ref a.b, n1.b, n2.b, n3.b, n4.b, n5.b, n6.b, n7.b, n8.b, n9.b);
            a.a = 1.0f;
        }

        public static void Compute(ref float[] filter, float[] source, int width, int height)
        {
            int l, r, b, t;

            int lI, rI, bI, tI;
            int bLI, bRI, tLI, tRI;

            int i, xx, yy;

            int widthM1 = width - 1;
            int widthM2 = width - 2;
            int heightM1 = height - 1;

            for (int x = 1; x < widthM1; x++)
            {
                l = x - 1;
                r = x + 1;

                bI = width + x;
                bLI = width + l;
                bRI = width + r;

                Blur(ref filter[x], source[x], source[l], source[r], source[bI], source[bI], source[bLI], source[bRI], source[bLI], source[bRI]);

                xx = Index(x, height - 1, width);
                l = xx - 1;
                r = xx + 1;

                bI = xx - width;
                bLI = l - width;
                bRI = r - width;

                Blur(ref filter[xx], source[xx], source[l], source[r], source[bI], source[bI], source[bLI], source[bRI], source[bLI], source[bRI]);
            }

            for (int y = 1; y < heightM1; y++)
            {
                i = Index(0, y, width);
                b = y - 1;
                t = y + 1;

                rI = Index(1, y, width);
                bI = Index(0, b, width);
                tI = Index(0, t, width);
                bRI = Index(1, b, width);
                tRI = Index(1, t, width);

                Blur(ref filter[i], source[i], source[rI], source[rI], source[bI], source[tI], source[bRI], source[bRI], source[tRI], source[tRI]);

                yy = Index(widthM1, y, width);
                b = y - 1;
                t = y + 1;

                rI = Index(widthM2, y, width);
                bI = Index(widthM1, b, width);
                tI = Index(widthM1, t, width);
                bRI = Index(widthM2, b, width);
                tRI = Index(widthM2, t, width);

                Blur(ref filter[yy], source[yy], source[rI], source[rI], source[bI], source[tI], source[bRI], source[bRI], source[tRI], source[tRI]);
            }

            for (int y = 1; y < heightM1; y++)
            {
                for (int x = 1; x < widthM1; x++)
                {
                    i = Index(x, y, width);
                    l = x - 1;
                    r = x + 1;
                    b = y - 1;
                    t = y + 1;

                    lI = Index(l, y, width);
                    rI = Index(r, y, width);
                    bI = Index(x, b, width);
                    tI = Index(x, t, width);
                    bLI = Index(l, b, width);
                    bRI = Index(r, b, width);
                    tLI = Index(l, t, width);
                    tRI = Index(r, t, width);

                    Blur(ref filter[i], source[i], source[lI], source[rI], source[bI], source[tI], source[bLI], source[bRI], source[tLI], source[tRI]);
                }
            }
        }

        public static void Compute(ref Color[] filter, Color[] source, int width, int height)
        {
            int l, r, b, t;

            int lI, rI, bI, tI;
            int bLI, bRI, tLI, tRI;

            int i, xx, yy;

            int widthM1 = width - 1;
            int widthM2 = width - 2;
            int heightM1 = height - 1;

            for (int x = 1; x < widthM1; x++)
            {
                l = x - 1;
                r = x + 1;

                bI = width + x;
                bLI = width + l;
                bRI = width + r;

                Blur(ref filter[x], source[x], source[l], source[r], source[bI], source[bI], source[bLI], source[bRI], source[bLI], source[bRI]);

                xx = Index(x, height - 1, width);
                l = xx - 1;
                r = xx + 1;

                bI = xx - width;
                bLI = l - width;
                bRI = r - width;

                Blur(ref filter[xx], source[xx], source[l], source[r], source[bI], source[bI], source[bLI], source[bRI], source[bLI], source[bRI]);
            }

            for (int y = 1; y < heightM1; y++)
            {
                i = Index(0, y, width);
                b = y - 1;
                t = y + 1;

                rI = Index(1, y, width);
                bI = Index(0, b, width);
                tI = Index(0, t, width);
                bRI = Index(1, b, width);
                tRI = Index(1, t, width);

                Blur(ref filter[i], source[i], source[rI], source[rI], source[bI], source[tI], source[bRI], source[bRI], source[tRI], source[tRI]);

                yy = Index(widthM1, y, width);
                b = y - 1;
                t = y + 1;

                rI = Index(widthM2, y, width);
                bI = Index(widthM1, b, width);
                tI = Index(widthM1, t, width);
                bRI = Index(widthM2, b, width);
                tRI = Index(widthM2, t, width);

                Blur(ref filter[yy], source[yy], source[rI], source[rI], source[bI], source[tI], source[bRI], source[bRI], source[tRI], source[tRI]);
            }

            for (int y = 1; y < heightM1; y++)
            {
                for (int x = 1; x < widthM1; x++)
                {
                    i = Index(x, y, width);
                    l = x - 1;
                    r = x + 1;
                    b = y - 1;
                    t = y + 1;

                    lI = Index(l, y, width);
                    rI = Index(r, y, width);
                    bI = Index(x, b, width);
                    tI = Index(x, t, width);
                    bLI = Index(l, b, width);
                    bRI = Index(r, b, width);
                    tLI = Index(l, t, width);
                    tRI = Index(r, t, width);

                    Blur(ref filter[i], source[i], source[lI], source[rI], source[bI], source[tI], source[bLI], source[bRI], source[tLI], source[tRI]);
                }
            }
        }

        public static void Compute(ref ComputeBuffer filter, ComputeBuffer source, int width, int height, bool isFloat)
        {
            Shader.SetInt("width", width);
            Shader.SetInt("height", height);

            int kernel = Shader.FindKernel(isFloat ? "BlurFloat" : "BlurColor");

            if (isFloat)
            {
                Shader.SetBuffer(kernel, "sourceFloat", source);
                Shader.SetBuffer(kernel, "filterFloat", filter);
            }
            else
            {
                Shader.SetBuffer(kernel, "sourceColor", source);
                Shader.SetBuffer(kernel, "filterColor", filter);
            }

            Shader.Dispatch(kernel, Mathf.CeilToInt(width / 32.0f), Mathf.CeilToInt(height / 32.0f), 1);
        }
         
        private static int Index(int x, int y, int width)
        {
            return y * width + x;
        }
    }
}
