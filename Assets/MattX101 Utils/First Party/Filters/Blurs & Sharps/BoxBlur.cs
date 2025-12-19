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
            int coordLeft, coordRight, coordBottom, coordTop;

            int index, indexSecondth;
            int indexLeft, indexRight, indexBottom, indexTop;
            int indexBottomLeft, indexBottomRight, indexTopLeft, indexTopRight;
            
            int widthMinus1 = width - 1;
            int widthMinus2 = width - 2;
            int heightMinus1 = height - 1;

            for (int x = 1; x < widthMinus1; x++)
            {
                coordLeft = x - 1;
                coordRight = x + 1;

                indexBottom = width + x;
                indexBottomLeft = width + coordLeft;
                indexBottomRight = width + coordRight;

                Blur(ref filter[x], source[x], source[coordLeft], source[coordRight], source[indexBottom], source[indexBottom], source[indexBottomLeft], source[indexBottomRight], source[indexBottomLeft], source[indexBottomRight]);

                indexSecondth = Index(x, height - 1, width);
                coordLeft = indexSecondth - 1;
                coordRight = indexSecondth + 1;

                indexBottom = indexSecondth - width;
                indexBottomLeft = coordLeft - width;
                indexBottomRight = coordRight - width;

                Blur(ref filter[indexSecondth], source[indexSecondth], source[coordLeft], source[coordRight], source[indexBottom], source[indexBottom], source[indexBottomLeft], source[indexBottomRight], source[indexBottomLeft], source[indexBottomRight]);
            }
            
            for (int y = 1; y < heightMinus1; y++)
            {
                index = Index(0, y, width);
                coordBottom = y - 1;
                coordTop = y + 1;

                indexRight = Index(1, y, width);
                indexBottom = Index(0, coordBottom, width);
                indexTop = Index(0, coordTop, width);
                indexBottomRight = Index(1, coordBottom, width);
                indexTopRight = Index(1, coordTop, width); 
                
                Blur(ref filter[index], source[index], source[indexRight], source[indexRight], source[indexBottom], source[indexTop], source[indexBottomRight], source[indexBottomRight], source[indexTopRight], source[indexTopRight]);

                indexSecondth = Index(widthMinus1, y, width);
                coordBottom = y - 1;
                coordTop = y + 1;

                indexRight = Index(widthMinus2, y, width);
                indexBottom = Index(widthMinus1, coordBottom, width);
                indexTop = Index(widthMinus1, coordTop, width);
                indexBottomRight = Index(widthMinus2, coordBottom, width);
                indexTopRight = Index(widthMinus2, coordTop, width);

                Blur(ref filter[indexSecondth], source[indexSecondth], source[indexRight], source[indexRight], source[indexBottom], source[indexTop], source[indexBottomRight], source[indexBottomRight], source[indexTopRight], source[indexTopRight]);
            }

            for (int y = 1; y < heightMinus1; y++)
            {
                for (int x = 1; x < widthMinus1; x++)
                {
                    index = Index(x, y, width);
                    coordLeft = x - 1;
                    coordRight = x + 1;
                    coordBottom = y - 1;
                    coordTop = y + 1;

                    indexLeft = Index(coordLeft, y, width);
                    indexRight = Index(coordRight, y, width);
                    indexBottom = Index(x, coordBottom, width);
                    indexTop = Index(x, coordTop, width);
                    indexBottomLeft = Index(coordLeft, coordBottom, width);
                    indexBottomRight = Index(coordRight, coordBottom, width);
                    indexTopLeft = Index(coordLeft, coordTop, width);
                    indexTopRight = Index(coordRight, coordTop, width);

                    Blur(ref filter[index], source[index], source[indexLeft], source[indexRight], source[indexBottom], source[indexTop], source[indexBottomLeft], source[indexBottomRight], source[indexTopLeft], source[indexTopRight]);
                }
            }
        }

        public static void Compute(ref Color[] filter, Color[] source, int width, int height)
        {
            int coordLeft, coordRight, coordBottom, coordTop;

            int index, indexSecondth;
            int indexLeft, indexRight, indexBottom, indexTop;
            int indexBottomLeft, indexBottomRight, indexTopLeft, indexTopRight;

            int widthMinus1 = width - 1;
            int widthMinus2 = width - 2;
            int heightMinus1 = height - 1;

            for (int x = 1; x < widthMinus1; x++)
            {
                coordLeft = x - 1;
                coordRight = x + 1;

                indexBottom = width + x;
                indexBottomLeft = width + coordLeft;
                indexBottomRight = width + coordRight;
                
                Blur(ref filter[x], source[x], source[coordLeft], source[coordRight], source[indexBottom], source[indexBottom], source[indexBottomLeft], source[indexBottomRight], source[indexBottomLeft], source[indexBottomRight]);

                indexSecondth = Index(x, height - 1, width);
                coordLeft = indexSecondth - 1;
                coordRight = indexSecondth + 1;

                indexBottom = indexSecondth - width;
                indexBottomLeft = coordLeft - width;
                indexBottomRight = coordRight - width;
                
                Blur(ref filter[indexSecondth], source[indexSecondth], source[coordLeft], source[coordRight], source[indexBottom], source[indexBottom], source[indexBottomLeft], source[indexBottomRight], source[indexBottomLeft], source[indexBottomRight]);
            }

            for (int y = 1; y < heightMinus1; y++)
            {
                index = Index(0, y, width);
                coordBottom = y - 1;
                coordTop = y + 1;

                indexRight = Index(1, y, width);
                indexBottom = Index(0, coordBottom, width);
                indexTop = Index(0, coordTop, width);
                indexBottomRight = Index(1, coordBottom, width);
                indexTopRight = Index(1, coordTop, width);
                
                Blur(ref filter[index], source[index], source[indexRight], source[indexRight], source[indexBottom], source[indexTop], source[indexBottomRight], source[indexBottomRight], source[indexTopRight], source[indexTopRight]);

                indexSecondth = Index(widthMinus1, y, width);
                coordBottom = y - 1;
                coordTop = y + 1;

                indexRight = Index(widthMinus2, y, width);
                indexBottom = Index(widthMinus1, coordBottom, width);
                indexTop = Index(widthMinus1, coordTop, width);
                indexBottomRight = Index(widthMinus2, coordBottom, width);
                indexTopRight = Index(widthMinus2, coordTop, width);

                Blur(ref filter[indexSecondth], source[indexSecondth], source[indexRight], source[indexRight], source[indexBottom], source[indexTop], source[indexBottomRight], source[indexBottomRight], source[indexTopRight], source[indexTopRight]);
            }

            for (int y = 1; y < heightMinus1; y++)
            {
                for (int x = 1; x < widthMinus1; x++)
                {
                    index = Index(x, y, width);
                    coordLeft = x - 1;
                    coordRight = x + 1;
                    coordBottom = y - 1;
                    coordTop = y + 1;

                    indexLeft = Index(coordLeft, y, width);
                    indexRight = Index(coordRight, y, width);
                    indexBottom = Index(x, coordBottom, width);
                    indexTop = Index(x, coordTop, width);
                    indexBottomLeft = Index(coordLeft, coordBottom, width);
                    indexBottomRight = Index(coordRight, coordBottom, width);
                    indexTopLeft = Index(coordLeft, coordTop, width);
                    indexTopRight = Index(coordRight, coordTop, width);

                    Blur(ref filter[index], source[index], source[indexLeft], source[indexRight], source[indexBottom], source[indexTop], source[indexBottomLeft], source[indexBottomRight], source[indexTopLeft], source[indexTopRight]);
                }
            }
        }

        public static void Compute(ref ComputeBuffer filter, ComputeBuffer source, int width, int height, bool isFloat)
        {
            Shader.SetInt(UnityEngine.Shader.PropertyToID("width"), width);
            Shader.SetInt(UnityEngine.Shader.PropertyToID("height"), height);

            int kernel = Shader.FindKernel(isFloat ? "BlurFloat" : "BlurColor");

            if (isFloat)
            {
                Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("sourceFloat"), source);
                Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("filterFloat"), filter);
            }
            else
            {
                Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("sourceColor"), source);
                Shader.SetBuffer(kernel, UnityEngine.Shader.PropertyToID("filterColor"), filter);
            }

            Shader.Dispatch(kernel, Mathf.CeilToInt(width / 32.0f), Mathf.CeilToInt(height / 32.0f), 1);
        }
         
        private static int Index(int x, int y, int width)
        {
            return y * width + x;
        }
    }
}
