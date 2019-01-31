using UnityEngine;

public class Emerald : LootableObject, IDropableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropEmeraldChance.Evaluate(-pos.y);
    
    protected override void OnLooted()
    {
        Score.Add(S.ScoreFromEmerald);
        Sounds.Points2.Play();
    }
}