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

public struct MathC
{
    // Custom Math Functions
    public static Vector3 NegaVectorX = new Vector3(-1, 1, 1);
    public static Vector3 NegaVectorY = new Vector3(1, -1, 1);
    public static Vector3 NegaVectorZ = new Vector3(1, 1, -1);

    public static Vector3 MultiplyVector(Vector3 v1, Vector3 v2)
    {
        return (new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z));
    }
}
