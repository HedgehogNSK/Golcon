using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Golcon
{
    [System.Serializable]
    public struct Range<T> where T: struct
    {
        public T Min { get; private set; }
        public T Max { get; private set; }

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }


    }
}