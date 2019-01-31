using UnityEngine;

public class ExtraTime : LootableObject, IDropableTile, IActionedTile
{
    static float _nextAvailableAfter;

    public bool RollForDrop(Vector2Int pos) =>
        Time.time > _nextAvailableAfter &&
        UnityEngine.Random.value <
            S.DropExtraTimeChance.Evaluate(-pos.y);

    public void PlacedOrDropped(Vector2Int pos, bool isDropped)
    {
        if (!isDropped) { return; }
        _nextAvailableAfter = Time.time + S.PowerupExtraTimeCooldown;
    }

    protected override void OnLooted()
    {
        Timer.TimeRemaining += S.PowerupExtraTimeAmount;
        Sounds.ExtraTime.Play();
    }

    protected override void Awake()
    {
        base.Awake();
        _nextAvailableAfter = 0f;
    }
}