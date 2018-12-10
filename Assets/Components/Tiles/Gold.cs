using UnityEngine;

public class Gold : LootableObject, IDropableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropGoldChance.Evaluate(-pos.y);
    
    protected override void OnLooted()
    {
        Score.Amount += S.ScoreFromGold;
        Sounds.Gold.Play();
    }
}