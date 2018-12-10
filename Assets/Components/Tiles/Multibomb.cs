using UnityEngine;

public class Multibomb : LootableObject, IDropableTile, IPlaceableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropMultibombChance.Evaluate(-pos.y);

    public bool RollForPlacement(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.PlaceMultibombChance.Evaluate(-pos.y);

    protected override void OnLooted()
    {
        ++Player.Multibomb;
        Sounds.Multibomb.Play();
    }
}