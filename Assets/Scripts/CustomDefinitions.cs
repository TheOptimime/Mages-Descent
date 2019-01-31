using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public struct DoubleTime
{
        public float cancelTime;
        public float defaultTime;

        public DoubleTime(float _cancelTime, float _defaultTime)
        {
            cancelTime = _cancelTime;
            defaultTime = _defaultTime;
        }

    public void Decrement()
    {
        cancelTime -= Time.deltaTime;
        defaultTime -= Time.deltaTime;
    }
}


