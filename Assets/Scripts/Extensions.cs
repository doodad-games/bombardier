using UnityEngine;

public static class Extensions
{
    public static Vector2Int IntPos(this MonoBehaviour b)
    {
        var pos = b.transform.position;
        return new Vector2Int((int)pos.x, (int)pos.y);
    }
}