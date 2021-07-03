using System.Collections;
using System.Linq;
using UnityEngine;

public class Mine : CustomTile, IDropableTile, IPlaceableTile, IActionedTile
{
    #pragma warning disable CS0649
    [SerializeField]
    Animator _bombFlashAnimator;
    #pragma warning restore CS0649

    float _explodeAfter;
    bool _fuseBurning;
    bool _combusting;

    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropMineChance.Evaluate(-pos.y);

    public bool RollForPlacement(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.PlaceMineChance.Evaluate(-pos.y);

    public void PlacedOrDropped(Vector2Int pos)
    {
        if (IsDropped || !S.MinePlacedHasReward) { return; }

        var options = new ICustomTile[]
            {   S.TileExtraBombPower
            ,   S.TileExtraSpeed
            ,   S.TileExtraTime
            ,   S.TileMultibomb
            };

        var minCol = pos.x - S.MineExplosionReach;
        var maxCol = pos.x + S.MineExplosionReach;
        var minRow = pos.y - S.MineExplosionReach;
        var maxRow = pos.y - 1;
        var positions = Enumerable.Range(minCol, maxCol - minCol)
            .SelectMany(
                col => Enumerable.Range(minRow, maxRow - minRow)
                    .Select(row => new Vector2Int(col, row))
            )
            .Where(
                _ => !TileController.Tiles.ContainsKey(_) ||
                    TileController.Tiles[_] == null
            )
            .ToArray();

        if (positions.Length == 0) { return; }

        var potential = positions[
            UnityEngine.Random.Range(0, positions.Length)
        ];

        TileController.SpawnTile(
            options[
                UnityEngine.Random.Range(0, options.Length)
            ],
            potential,
            false
        );
    }

    public override void Initialise(bool isDropped)
    {
        base.Initialise(isDropped);

        _explodeAfter = S.MineExplodeAfter *
            (isDropped ? 1 : S.MinePlaceDurationMultiplier);

        if (isDropped) { BurnFuse(); }
    }

    public void BurnFuse()
    {
        if (_fuseBurning) { return; }
        StartCoroutine(_BurnFuse());
    }

    public override void Combust()
    {
        if (_combusting) { return; }
        StartCoroutine(_Combust());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsDropped || _fuseBurning || _combusting) { return; }

        var player = other.GetComponent<Player>();
        if (player == null) { return; }

        BurnFuse();
    }

    IEnumerator _BurnFuse()
    {
        if (_fuseBurning) { yield break; }
        _fuseBurning = true;

        _bombFlashAnimator.SetFloat(
            "PlaybackSpeed",
            1 / (_explodeAfter + S.MineDestroyAfter)
        );
        _bombFlashAnimator.SetTrigger("Burn");

        Sounds.MineTrigger.Play();

        yield return new WaitForSeconds(
            _explodeAfter * S.MineCombustAt
        );

        if (_combusting) { yield break; }
        StartCoroutine(_Combust());
    }

    IEnumerator _Combust()
    {
        if (_combusting) { yield break; }
        _combusting = true;

        yield return new WaitForSeconds(
            _explodeAfter * (1 - S.MineCombustAt)
        );

        ++Stats.MinesDetonated;

        _bombFlashAnimator.Play(
            "BombFlash",
            0,
            S.MineCombustAt
        );

        Instantiate(
            S.MineExplosion,
            transform.position,
            Quaternion.identity
        )
            .GetComponent<Explosion>()
            .source = Explosion.Source.Mine;
        Sounds.Explode();

        Destroy(gameObject, S.MineDestroyAfter);
    }
}