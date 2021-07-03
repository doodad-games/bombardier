using System;
using System.Linq;
using UnityEngine;

public class PlayerExplosion : Explosion
{
    static Vector2Int _up = new Vector2Int(0, 1);
    static Vector2Int _right = new Vector2Int(1, 0);
    static Vector2Int _down = new Vector2Int(0, -1);
    static Vector2Int _left = new Vector2Int(-1, 0);

    #pragma warning disable CS0649
    [SerializeField]
    SpriteRenderer _horizontal;
    [SerializeField]
    BoxCollider2D _horizontalCollider;
    [SerializeField]
    SpriteRenderer _vertical;
    [SerializeField]
    BoxCollider2D _verticalCollider;
    #pragma warning restore CS0649

    bool _initialised;
    Vector2 _safeArea;
    int _power;
    int _powerSpawned;
    float _startTime;
    Vector2Int _intPos;
    float _fanOutTime;
    float _burnTilesAfter;

    Extent[] _extents;
    int _minX;
    int _maxX;
    int _minY;
    int _maxY;

    public void Initialise(int power)
    {
        if (_initialised) { throw new NotSupportedException(); }

        source = Source.PlayerBomb;
        _power = power;

        _initialised = true;
    }

    protected override void Start()
    {
        if (!_initialised) { throw new NotSupportedException(); }
        
        base.Start();
        
        _safeArea = Vector2.one * S.ExplosionSafeDistance * 2;
        _powerSpawned = 1;
        _startTime = Time.time;
        _burnTilesAfter = S.ExplosionLifetime *
            S.ExplosionEndBurnAt;
        _fanOutTime = S.PlayerExplosionFanOutTime;

        var pos = transform.position;
        _intPos = new Vector2Int((int)pos.x, (int)pos.y);
        _minX = _intPos.x - 1;
        _maxX = _intPos.x + 1;
        _minY = _intPos.y - 1;
        _maxY = _intPos.y + 1;

        var pierces = _power >= S.PowerupBombPowerLimit ? 1 : 0;

        _extents = new Vector2Int[] { _up , _right , _down , _left }
            .Select(_ => ProcessTile(
                new Extent
                {   direction = _
                ,   power = _power
                ,   pierces = pierces
                },
                TileController.Tiles[_intPos + _]
            )
            )
            .ToArray();
    }

    void Update()
    {
        var timePassed = Time.time - _startTime;
        var fanOutPct = Mathf.Min(1f, timePassed / _fanOutTime);

        var powerToSpawn = 1 + Mathf.FloorToInt(
            (_power - 1) * fanOutPct
        );

        if (powerToSpawn != _powerSpawned)
        {
            for (var e = 0; e != _extents.Length; ++e)
            {
                var extent = _extents[e];
                if (extent.capped) { continue; }

                for (var i = _powerSpawned; i != powerToSpawn; ++i)
                {
                    Vector2Int posToCheck;

                    if (extent.direction.x == 0)
                    {
                        var up = extent.direction.y == 1;

                        if (up) { ++_maxY; }
                        else { --_minY; }

                        posToCheck = new Vector2Int(
                            _intPos.x, up ? _maxY : _minY
                        );
                    }
                    else
                    {
                        var right = extent.direction.x == 1;

                        if (right) { ++_maxX; }
                        else { --_minX; }

                        posToCheck = new Vector2Int(
                            right ? _maxX : _minX, _intPos.y
                        );
                    }

                    ProcessTile(
                        extent,
                        TileController.Tiles[posToCheck]
                    );
                    
                    if (extent.capped) { break; }
                }
            }

            _powerSpawned = powerToSpawn;
        }

        var grow = _power * Time.deltaTime / _fanOutTime;

        var fullWidth = _maxX - _minX + 1;
        if (_horizontal.size.x < fullWidth)
        {
            _horizontal.size = new Vector2(
                Mathf.Min(
                    fullWidth,
                    _horizontal.size.x + grow
                ),
                _horizontal.size.y
            );
            _horizontalCollider.size =
                _horizontal.size - _safeArea;

            var pos = _horizontal.transform.position;
            var width = _horizontal.size.x / 2f;
            var push = 0f;

            var leftPos = pos.x + 0.5f - width;
            var rightPos = pos.x - 0.5f + width;

            if (leftPos < _minX) { push = leftPos - _minX; }
            else if (rightPos > _maxX) { push = rightPos - _maxX; }

            if (push != 0f)
            {
                _horizontal.transform.position = new Vector3(
                    pos.x - push,
                    pos.y,
                    pos.z
                );
            }
        }

        var fullHeight = _maxY - _minY + 1;
        if (_vertical.size.x < fullHeight)
        {
            _vertical.size = new Vector2(
                Mathf.Min(
                    fullHeight,
                    _vertical.size.x + grow
                ),
                _vertical.size.y
            );
            _verticalCollider.size = 
                _vertical.size - _safeArea;

            var pos = _vertical.transform.position;
            var height = _vertical.size.x / 2f;
            var push = 0f;

            var botPos = pos.y + 0.5f - height;
            var topPos = pos.y - 0.5f + height;

            if (botPos < _minY) { push = botPos - _minY; }
            else if (topPos > _maxY) { push = topPos - _maxY; }

            if (push != 0f)
            {
                _vertical.transform.position = new Vector3(
                    pos.x,
                    pos.y - push,
                    pos.z
                );
            }
        }
    }

    Extent ProcessTile(Extent extent, ICustomTile tile)
    {
        if (tile != null)
        {
            StartCoroutine(Combust(tile));

            if (tile.NeedsPierce())
            {
                if (extent.pierces == 0) { extent.capped = true; }
                else
                {
                    if (!extent.piercing) { extent.piercing = true; }
                    --extent.pierces;
                }
            }
            if (tile.CapsExplosions()) { extent.capped = true; }
        }
        else if (extent.piercing)
        {
            if (extent.pierces == 0) { extent.capped = true; }
            else { --extent.pierces; }
        }
        else if (extent.power == 1) { extent.capped = true; }
        else { --extent.power; }
        
        return extent;
    }

    class Extent
    {
        public Vector2Int direction;
        public int power;
        public bool piercing;
        public int pierces;
        public bool capped;
    }
}
