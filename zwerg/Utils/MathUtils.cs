using System;

namespace Utils
{
    class MathUtils
    {
        public static float Clamp(float min, float max, float value)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        public static void FFT(float[] pX, float[] pY, int steps, bool inverse)
        {
            inverse = !inverse;

            int len = 1 << steps;
            int len2 = len >> 1;

            float tX, tY;
            int j = 0, k;

            for(int i = 0; i < len - 1; i ++)
            {
                if(i < j)
                {
                    tX = pX[i];
                    tY = pY[i];
                    pX[i] = pX[j];
                    pY[i] = pY[j];
                    pX[j] = tX;
                    pY[j] = tY;
                }

                k = len2;

                while(k <= j)
                {
                    j -= k;
                    k >>= 1;
                }

                j += k;
            }

            float con = -6.28318530717958647692f / len;
            if(inverse) con *= -1;

            int deltaW = len >> 1;
            int wIndex;
            int dist;

            for(int span = 1; span < len; span = dist)
            {
                dist = span << 1;
                wIndex = 0;

                for(int m = 0; m < span; m ++)
                {
                    float ang = wIndex * con;
                    float co = (float)Math.Cos(ang);
                    float si = (float)Math.Sin(ang);

                    for(int i = m; i < len; i += dist)
                    {
                        tX = pX[i + span] * co - pY[i + span] * si;
                        tY = pX[i + span] * si + pY[i + span] * co;

                        pX[i + span] = pX[i] - tX;
                        pY[i + span] = pY[i] - tY;
                        pX[i] = pX[i] + tX;
                        pY[i] = pY[i] + tY;
                    }

                    wIndex += deltaW;
                }

                deltaW >>= 1;
            }

            if(inverse)
            {
                con = 1.0f / len;
                for(int i = 0; i < len; i ++)
                {
                    pX[i] *= con;
                    pY[i] *= con;
                }
            }
        }         
    
    }
}

