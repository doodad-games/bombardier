using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    static TileController _instance;

    static TileController() =>
        Player.SubMove((a, b) => _instance.RenderForMovement(a, b));

    public static IReadOnlyDictionary<Vector2Int, ICustomTile> Tiles =>
        _instance._tiles;

    public static ICustomTile SpawnTile(ICustomTile tile, Vector2Int pos) =>
        _instance._SpawnTile(tile, pos);

    Dictionary<Vector2Int, ICustomTile> _tiles;
    List<IPlaceableTile> _prioritisedTiles;

    Vector2Int _lastRenderedPos;

    void Awake() =>
        _instance = this;

    void Start()
    {
        _tiles = new Dictionary<Vector2Int, ICustomTile>();
        _prioritisedTiles = new List<IPlaceableTile>
        {   S.TileMine
        ,   S.TileExtraBombPower
        ,   S.TileExtraLife
        ,   S.TileExtraSpeed
        ,   S.TileExtraTime
        ,   S.TileMultibomb
        ,   S.TileIndestructibleRock
        ,   S.TileRock
        };

        _lastRenderedPos = Player.PreMovePos;

        var distX = S.TilesSpawnDistanceX;
        var distY = S.TilesSpawnDistanceY;

        var minCol = _lastRenderedPos.x - distX;
        var maxCol = _lastRenderedPos.x + distX;
        var minRow = _lastRenderedPos.y - distY;
        var maxRow = _lastRenderedPos.y + distY;
        for (var col = minCol; col <= maxCol; ++col)
        {
            for (var row = minRow; row <= maxRow; ++row)
            {
                var pos = new Vector2Int(col, row);
                if (pos == _lastRenderedPos)
                {
                    _tiles[pos] = null;
                    continue;
                }
                SpawnTile(pos);
            }
        }
    }

    void SpawnTile(Vector2Int pos)
    {
        if (_tiles.ContainsKey(pos)) { return; }

        IPlaceableTile tile = null;

        if (pos.y > 0)
        {
            _tiles[pos] = tile = S.TileIndestructibleRock;
        }
        else if (pos.y != 0)
        {
            foreach (var potential in _prioritisedTiles)
            {
                if (potential.RollForPlacement(pos))
                {
                    tile = potential;
                    break;
                }
            }
        }

        if (tile != null) { _SpawnTile(tile, pos); }
        else { _tiles[pos] = null; }
    }

    ICustomTile _SpawnTile(ICustomTile tile, Vector2Int pos)
    {
        var retval = Instantiate(
            tile.GameObject(),
            new Vector3(pos.x, pos.y, 0f),
            Quaternion.identity,
            transform
        ).GetComponent<CustomTile>();
        
        _tiles[pos] = retval;
        retval.Initialise(false);
        retval.SubOnDestroy(() => _tiles[pos] = null);

        if (retval is IActionedTile)
        {
            (retval as IActionedTile).PlacedOrDropped(pos, false);
        }

        return retval;
    }

    void RenderForMovement(Vector2Int from, Vector2Int to)
    {
        if (from != _lastRenderedPos)
        {
            throw new System.NotSupportedException();
        }

        var d = to - from;
        var positions = new HashSet<Vector2Int>();
        
        var distX = S.TilesSpawnDistanceX;
        var distY = S.TilesSpawnDistanceY;
        
        if (d.x != 0)
        {
            var minCol = d.x > 0
                ? from.x + distX
                : from.x - distX + d.x;
            var maxCol = minCol + Mathf.Abs(d.x);
            var minRow = from.y - distY;
            var maxRow = from.y + distY;
            for (var col = minCol; col <= maxCol; ++col)
            {
                for (var row = minRow; row <= maxRow; ++row)
                {
                    positions.Add(new Vector2Int(col, row));
                }
            }
        }
        if (d.y != 0)
        {
            var minCol = from.x - distX;
            var maxCol = from.x + distX;
            var minRow = d.y > 0
                ? from.y + distY
                : from.y - distY + d.y;
            var maxRow = minRow + Mathf.Abs(d.y);
            for (var col = minCol; col <= maxCol; ++col)
            {
                for (var row = minRow; row <= maxRow; ++row)
                {
                    positions.Add(new Vector2Int(col, row));
                }
            }
        }

        foreach (var pos in positions) { SpawnTile(pos); }

        _lastRenderedPos = to;
    }
}
