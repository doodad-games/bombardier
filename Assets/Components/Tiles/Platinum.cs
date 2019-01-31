using UnityEngine;

public class Platinum : LootableObject, IDropableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropPlatinumChance.Evaluate(-pos.y);
    
    protected override void OnLooted()
    {
        Score.Add(S.ScoreFromPlatinum);
        Sounds.Points1.Play();
    }
}