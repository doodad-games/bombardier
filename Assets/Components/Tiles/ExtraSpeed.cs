using UnityEngine;

public class ExtraSpeed : LootableObject, IDropableTile, IPlaceableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropExtraSpeedChance.Evaluate(-pos.y);

    public bool RollForPlacement(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.PlaceExtraSpeedChance.Evaluate(-pos.y);

    protected override void OnLooted()
    {
        ++Player.ExtraSpeed;
        Sounds.ExtraSpeed.Play();
    }
}