using UnityEngine;

public class Sapphire : LootableObject, IDropableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropSapphireChance.Evaluate(-pos.y);
    
    protected override void OnLooted()
    {
        Score.Add(S.ScoreFromSapphire);
        Sounds.Points2.Play();
    }
}