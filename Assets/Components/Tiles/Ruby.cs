using UnityEngine;

public class Ruby : LootableObject, IDropableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropRubyChance.Evaluate(-pos.y);
    
    protected override void OnLooted()
    {
        Score.Add(S.ScoreFromRuby);
        Sounds.Points2.Play();
    }
}