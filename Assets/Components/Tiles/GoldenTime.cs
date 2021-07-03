using UnityEngine;

public class GoldenTime : LootableObject, IDropableTile, IActionedTile
{
    static float _nextAvailableAfter;

    public bool RollForDrop(Vector2Int pos) =>
        Time.time > _nextAvailableAfter &&
        UnityEngine.Random.value <
            S.DropGoldenTimeChance.Evaluate(-pos.y);

    public void PlacedOrDropped(Vector2Int pos) =>
        Sounds.SuperValuableDrop.Play();

    protected override void OnLooted()
    {
        Timer.TimeRemaining += S.PowerupGoldenTimeAmount;
        Sounds.GoldenTime.Play();
    }

    protected override void Awake()
    {
        base.Awake();
        _nextAvailableAfter = 0f;
    }
}