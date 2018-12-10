using UnityEngine;

public class Diamond : LootableObject, IDropableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropDiamondChance.Evaluate(-pos.y);
    
    protected override void OnLooted()
    {
        Score.Amount += S.ScoreFromDiamond;
        Sounds.Diamond.Play();
    }
}