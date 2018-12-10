using UnityEngine;

public interface IActionedTile : ICustomTile
{
    void PlacedOrDropped(Vector2Int pos, bool isDropped);
}