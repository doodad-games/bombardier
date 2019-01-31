using UnityEngine;

public class ExtraBombPower : LootableObject, IDropableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropExtraBombPowerChance.Evaluate(-pos.y);

    protected override void OnLooted()
    {
        Player.BombPower += 1;
        Sounds.ExtraBombPower.Play();
    }
}