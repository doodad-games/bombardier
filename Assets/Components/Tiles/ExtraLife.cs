using UnityEngine;

public class ExtraLife : LootableObject, IDropableTile, IActionedTile
{
    static float _nextAvailableAfter;

    public bool RollForDrop(Vector2Int pos) =>
        Time.time > _nextAvailableAfter &&
        UnityEngine.Random.value <
            S.DropExtraLifeChance.Evaluate(-pos.y);

    public void PlacedOrDropped(Vector2Int pos, bool isDropped)
    {
        if (!isDropped) { return; }
        _nextAvailableAfter = Time.time + S.PowerupExtraLifeCooldown;
    }

    protected override void OnLooted()
    {
        Player.HasExtraLife = true;
        Sounds.ExtraLife.Play();
    }

    protected override void Awake()
    {
        base.Awake();
        _nextAvailableAfter = 0f;
    }
}