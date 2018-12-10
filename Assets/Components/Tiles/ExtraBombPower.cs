using UnityEngine;

public class ExtraBombPower : LootableObject, IDropableTile, IPlaceableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropExtraBombPowerChance.Evaluate(-pos.y);

    public bool RollForPlacement(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.PlaceExtraBombPowerChance.Evaluate(-pos.y);

    protected override void OnLooted()
    {
        Player.BombPower += 1;
        Sounds.ExtraBombPower.Play();
    }
}