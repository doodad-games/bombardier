using UnityEngine;

public class Emerald : LootableObject, IDropableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropEmeraldChance.Evaluate(-pos.y);
    
    protected override void OnLooted()
    {
        Score.Amount += S.ScoreFromEmerald;
        Sounds.Emerald.Play();
    }
}