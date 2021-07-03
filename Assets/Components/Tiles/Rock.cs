using UnityEngine;

public class Rock : CustomTile, IPlaceableTile
{
    static IDropableTile[] _dropableTiles;

    public bool preventDrops;
    
    public override bool Unwalkable() => true;
    public override bool NeedsPierce() => true;
    public override bool GoneAfterCombust() => false;

    public bool RollForPlacement(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.PlaceRockChance.Evaluate(-pos.y);
    
    protected override void OnBurn()
    {
        ++Stats.RocksDestroyed;

        CleanUp();

        if (!preventDrops)
        {
            var pos = transform.position;
            var intPos = new Vector2Int((int)pos.x, (int)pos.y);

            IDropableTile tile = null;

            foreach (var potential in _dropableTiles)
            {
                if (potential.RollForDrop(intPos))
                {
                    tile = potential;
                    break;
                }
            }

            if (tile != null)
                TileController.SpawnTile(tile, intPos, true);
        }

        Destroy(gameObject);
    }
    
    protected override void Awake()
    {
        base.Awake();

        if (_dropableTiles == null)
        {
            _dropableTiles = new IDropableTile[]
            {   S.TileExtraLife
            ,   S.TileExtraBombPower
            ,   S.TileMine
            ,   S.TileMultibomb
            ,   S.TileExtraSpeed
            ,   S.TileExtraTime
            ,   S.TileGoldenTime
            ,   S.TileBronze
            ,   S.TileDiamond
            ,   S.TileEmerald
            ,   S.TileGold
            ,   S.TilePinkDiamond
            ,   S.TilePlatinum
            ,   S.TileRuby
            ,   S.TileSapphire
            ,   S.TileSilver
            };
        }
    }
}