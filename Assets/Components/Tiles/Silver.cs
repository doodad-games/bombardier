using UnityEngine;

public class Silver : LootableObject, IDropableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropSilverChance.Evaluate(-pos.y);
    
    protected override void OnLooted()
    {
        Score.Add(S.ScoreFromSilver);
        Sounds.Points1.Play();
    }
}