using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hedge
{
    namespace Tools
    {
        public static class FormatGameText

        {
            private static readonly List<string> myNum;

            static FormatGameText()
            {
                char prefix = ' ';
                myNum = new List<string>();
                myNum.Add("");
                myNum.Add(prefix + "K");
                myNum.Add(prefix + "M");
                myNum.Add(prefix + "B");
                myNum.Add(prefix + "T");
                myNum.Add(prefix + "q");
                myNum.Add(prefix + "Q");
                myNum.Add(prefix + "s");
                // ....
            }

            public static string ToShortNumber(this float value)
            {
                string initValue = value.ToString();
                int num = 0;
                while (value >= 1000)
                {
                    num++;
                    value /= 1000;
                }

                string format;
                if (value % (int)value < 0.1f || value == 0)
                    format = string.Format("{0:0}{1}", value, myNum[num]);
                else
                    format = string.Format("{0:F1}{1}", value, myNum[num]);
                return format;
            }
        }
        public static class VectorExtension
        {
            public static Vector2 Rotate(this Vector2 vector, float angle)
            {
                float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
                float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
                return new Vector2(cos * vector.x - sin * vector.y, sin * vector.x + cos * vector.y);
            }
        }

    }
}
