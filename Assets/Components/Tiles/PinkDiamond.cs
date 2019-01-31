using UnityEngine;

public class PinkDiamond : LootableObject, IDropableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropPinkDiamondChance.Evaluate(-pos.y);
    
    protected override void OnLooted()
    {
        Score.Add(S.ScoreFromPinkDiamond);
        Sounds.Points3.Play();
    }
}