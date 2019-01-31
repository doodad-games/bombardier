using UnityEngine;

public class ExtraSpeed : LootableObject, IDropableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropExtraSpeedChance.Evaluate(-pos.y);

    protected override void OnLooted()
    {
        ++Player.ExtraSpeed;
        Sounds.ExtraSpeed.Play();
    }
}