using UnityEngine;

public interface IPlaceableTile : ICustomTile
{
    bool RollForPlacement(Vector2Int pos);
}