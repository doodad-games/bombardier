using UnityEngine;

public class IndestructibleRock : CustomTile, IPlaceableTile
{
    public override bool Unwalkable() => true;
    public override bool CapsExplosions() => true;
    public override bool GoneAfterCombust() => false;

    public bool RollForPlacement(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.PlaceIndestructibleRockChance.Evaluate(-pos.y);
}