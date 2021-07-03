using UnityEngine;

public class MineExplosion : Explosion
{
    float _burnTilesAfter;

    protected override void Start()
    {
        base.Start();

        var fullSize = Vector2.one * (1 + S.MineExplosionReach * 2);

        GetComponent<SpriteRenderer>().size = fullSize;
        GetComponent<BoxCollider2D>().size =
            fullSize - Vector2.one * S.ExplosionSafeDistance * 2;

        _burnTilesAfter = Time.time +
            S.ExplosionLifetime * S.ExplosionEndBurnAt;

        var pos = this.IntPos();
        var minCol = pos.x - S.MineExplosionReach;
        var maxCol = pos.x + S.MineExplosionReach;
        var minRow = pos.y - S.MineExplosionReach;
        var maxRow = pos.y + S.MineExplosionReach;
        for (var col = minCol; col <= maxCol; ++col)
        {
            for (var row = minRow; row <= maxRow; ++row)
            {
                var tile = TileController.Tiles[
                    new Vector2Int(col, row)
                ];
                if (tile == null) { continue; }

                StartCoroutine(Combust(tile, true));
            }
        }
    }
}