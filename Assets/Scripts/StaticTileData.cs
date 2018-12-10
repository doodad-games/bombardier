using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomTile", menuName = "StaticTileData")]
public class StaticTileData : ScriptableObject
{
    #pragma warning disable CS0649
    [SerializeField]
    List<Sprite> _sprites;
    #pragma warning restore CS0649

    public IReadOnlyList<Sprite> Sprites => _sprites;
}