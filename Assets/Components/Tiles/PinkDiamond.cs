using UnityEngine;

public class PinkDiamond : LootableObject, IDropableTile, IActionedTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropPinkDiamondChance.Evaluate(-pos.y);

    public void PlacedOrDropped(Vector2Int pos) =>
        Sounds.SuperValuableDrop.Play();
    
    protected override void OnLooted()
    {
        Score.Add(S.ScoreFromPinkDiamond);
        Sounds.Points3.Play();
    }
}