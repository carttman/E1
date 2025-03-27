using UnityEngine;

public static class FunctionLibrary
{
    public static Vector3 Bezier(Vector3 t1, Vector3 t2, Vector3 t3, float t)
    {
        return Vector3.Lerp(Vector3.Lerp(t1, t2, t), Vector3.Lerp(t2, t3, t), t);
    }
}
