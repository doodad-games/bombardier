using UnityEngine;

public interface IDropableTile : ICustomTile
{
    bool RollForDrop(Vector2Int pos);
}