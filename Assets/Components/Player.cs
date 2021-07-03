using System;
using System.Collections.Generic;
using MyLibrary;
using UnityEngine;

public class Player : MonoBehaviour
{
    static IReadOnlyDictionary<Vector2Int, float> ZRotationForMovement =
        new Dictionary<Vector2Int, float>
        {   { new Vector2Int(1, 0), 270f }
        ,   { new Vector2Int(1, 1), 315f }
        ,   { new Vector2Int(0, 1), 0f }
        ,   { new Vector2Int(-1, 1), 45f }
        ,   { new Vector2Int(-1, 0), 90f }
        ,   { new Vector2Int(-1, -1), 135f }
        ,   { new Vector2Int(0, -1), 180f }
        ,   { new Vector2Int(1, -1), 225f }
        };

    public static event Action<int> onMultibombChanged;
    public static Action<bool> onExtraLifeChanged;
    public static event Action<CauseOfGameOver> onGameOver;
    public static event Action<Vector2Int> onMove;

    public static Player Instance { get; private set; }
    public static CauseOfGameOver? GameOver { get; private set; }
    public static bool IsDeepest { get; private set; }

    static bool _hasExtraLife;
    static int _bombPower;
    static int _extraSpeed;
    static float _moveTime;
    static float _maxRotPerSecond;
    static int _multibomb;
    
    public static int BombPower
    {
        get { return _bombPower; }
        set
        {
            _bombPower = Mathf.Min(
                value,
                S.PowerupBombPowerLimit
            );
        }
    }

    public static bool HasExtraLife
    {
        get { return _hasExtraLife; }
        set
        {
            if (value == _hasExtraLife) { return; }
            _hasExtraLife = value;
            onExtraLifeChanged?.Invoke(value);
        }
    }
    
    public static int ExtraSpeed
    {
        get { return _extraSpeed; }
        set
        {
            _extraSpeed = Mathf.Min(
                value,
                S.PowerupExtraSpeedLimit
            );

            _moveTime = S.PlayerMoveTime -
                value * S.PowerupExtraSpeedDifference;
            _maxRotPerSecond = S.PlayerMoveMaxRotPerSecond *
                (S.PlayerMoveTime / _moveTime);
            
            Instance._animator.SetFloat(
                "MovePlaybackSpeed", 1 / _moveTime
            );
        }
    }

    public static int Multibomb
    {
        get { return _multibomb; }
        set
        {
            if (value == _multibomb) { return; }

            _multibomb = Mathf.Min(
                value,
                S.PowerupMultibombLimit
            );

            onMultibombChanged?.Invoke(value);
        }
    }

    public static Vector2Int Pos
    {
        get => Instance._tilePos;
        private set
        {
            Instance._tilePos = value;
            onMove?.Invoke(value);
        }
    }

    public static Vector2Int? ClosestEmptyTile =>
        Instance._ClosestEmptyTile();
    
    static Player()
    {
        Timer.onElapsed += () =>
            Instance.EndGame(CauseOfGameOver.Time);

        GameplayUI.onMenuHidden += () =>
            Instance.enabled = true;
    }
    
    #pragma warning disable CS0649
    [SerializeField]
    Transform _camera;
    [SerializeField]
    Animator _animator;
    [SerializeField]
    Transform _toRotate;
    #pragma warning restore CS0649
    
    float _safeUntil;
    Vector2Int _tilePos;
    Vector2Int _lastRotation;
    int _lastInputH;
    int _lastInputV;
    float _hPressedUntil;
    float _vPressedUntil;
    int _lastHDir;
    int _lastVDir;
    Quaternion _rotateTo;
    int _deepest;

    public void Burn(Explosion.Source source)
    {
        if (GameOver != null || Time.time < _safeUntil) { return; }

        if (_hasExtraLife)
        {
            HasExtraLife = false;

            _safeUntil = Time.time + S.PowerupExtraLifeSaveDuration;

            Sounds.LoseLife.Play();
            ++Stats.ExtraLivesLost;
            return;
        }

        _animator.SetTrigger("Burn");
        Sounds.Burn();


        Instantiate(
            S.PlayerAsh,
            transform.position,
            Quaternion.identity
        )
            .GetComponent<Animator>()
            .SetFloat("PlaybackSpeed", 1 / (S.PlayerBurnTime * 2));
        
        _camera.parent = null;

        Destroy(gameObject, S.PlayerBurnTime);

        if (source == Explosion.Source.PlayerBomb)
        {
            ++Stats.LossesByOwnBomb;
            EndGame(CauseOfGameOver.PlayerBomb);
        }
        else if (source == Explosion.Source.Mine)
        {
            ++Stats.LossesByMines;
            EndGame(CauseOfGameOver.Mine);
        }
    }

    void Awake()
    {
        Time.timeScale = 1f;

        Instance = this;
        GameOver = null;
        IsDeepest = false;
    }

    void Start()
    {
        BombPower = 1;
        ExtraSpeed = 0;
        _multibomb = 0;

        _animator.SetFloat("BurnPlaybackSpeed", 1 / S.PlayerBurnTime);

        var curPos = transform.position;
        _tilePos = new Vector2Int(
            (int)curPos.x,
            (int)curPos.y
        );

        _lastRotation = new Vector2Int(0, 1);

        if (GameplayUI.MenuShowing) enabled = false;
    }

    void EndGame(CauseOfGameOver cause)
    {
        if (GameOver != null) { return; }

        if (Score.IsHighscore)
            Sounds.HighScore.Play();
        else if (
            cause == CauseOfGameOver.PlayerBomb ||
            cause == CauseOfGameOver.Mine
        )
            Sounds.PlayerBurn.Play();
        else
            Sounds.TimeOut.Play();

        Time.timeScale = 0.5f;
        _animator.SetFloat("MovePlaybackSpeed", 0f);
        GameOver = cause;
        onGameOver?.Invoke(cause);

        enabled = false;
        Sounds.Movement.Pause();
        KVS.Save();
    }

    Vector2Int? _ClosestEmptyTile()
    {
        var pos = transform.position;

        var minCol = Mathf.FloorToInt(pos.x);
        var maxCol = Mathf.CeilToInt(pos.x);
        var numCols = maxCol - minCol + 1;
        var minRow = Mathf.FloorToInt(pos.y);
        var maxRow = Mathf.CeilToInt(pos.y);
        var numRows = maxRow - minRow + 1;

        var positions = new Tuple<Vector2Int, float>[numCols * numRows];

        for (var i = 0; i != numCols; ++i)
        {
            var col = minCol + i;

            for (var j = 0; j != numRows; ++j)
            {
                var row = minRow + j;

                positions[i * numRows + j] =
                    Tuple.Create(
                        new Vector2Int(col, row),
                        Vector3.Distance(
                            new Vector3(
                                col, row, 0f
                            ),
                            pos
                        )
                    );
            }
        }

        Array.Sort(positions, (a, b) =>
        {
            return TileController.Tiles[b.Item1] == null
                ? (
                    TileController.Tiles[a.Item1] == null
                        ? a.Item2.CompareTo(b.Item2)
                        : 1
                )
                : -1;
        });

        var potential = positions[0].Item1;
        return TileController.Tiles[potential] == null
            ? potential
            : (Vector2Int?)null;
    }

    void Update()
    {
        var h = Joystick.Horizontal + Input.GetAxis("Horizontal");
        var dh = h == 0f ? 0 : Mathf.RoundToInt(h);
        if (_lastInputH == 0)
        {
            if (dh != 0)
                _hPressedUntil = Time.time + _moveTime * 0.5f;
        }
        else
        {
            if (dh == 0 && Time.time < _hPressedUntil) dh = _lastInputH;
            else if (dh != _lastInputH) _hPressedUntil = 0f;
        }
        
        var v = Joystick.Vertical + Input.GetAxis("Vertical");
        var dv = v == 0f ? 0 : Mathf.RoundToInt(v);
        if (_lastInputV == 0)
        {
            if (dv != 0)
                _vPressedUntil = Time.time + _moveTime * 0.5f;
        }
        else
        {
            if (dv == 0 && Time.time < _vPressedUntil) dv = _lastInputV;
            else if (dv != _lastInputV) _vPressedUntil = 0f;
        }

        int actualH;
        int actualV;
        var hFirst = Mathf.Abs(h) > Mathf.Abs(v);
        if (hFirst)
        {
            actualH = Move(dh, true, Time.deltaTime);
            if (actualH != 0) _lastHDir = actualH;

            actualV = Move(dv, false, Time.deltaTime);
            if (actualV != 0) _lastVDir = actualV;
        }
        else
        {
            actualV = Move(dv, false, Time.deltaTime);
            if (actualV != 0) _lastVDir = actualV;

            actualH = Move(dh, true, Time.deltaTime);
            if (actualH != 0) _lastHDir = actualH;
        }

        if (
            (dh != 0 && actualH != 0) ||
            (dv != 0 && actualV != 0)
        )
        {
            var rotation = new Vector2Int(
                dh == 0 ? 0 : actualH,
                dv == 0 ? 0 : actualV
            );
            if (rotation != _lastRotation)
            {
                _lastRotation = rotation;
                _rotateTo = Quaternion.Euler(0f, 0f, ZRotationForMovement[rotation]);
            }

            _animator.SetBool("Moving", true);
            if (!Sounds.Movement.isPlaying) Sounds.Movement.Play();
        }
        else
        {
            _animator.SetBool("Moving", false);
            Sounds.Movement.Pause();
        }

        if (_toRotate.rotation != _rotateTo)
        {
            _toRotate.rotation = Quaternion.RotateTowards(
                _toRotate.rotation,
                _rotateTo,
                _maxRotPerSecond * Time.deltaTime
            );
        }

        _lastInputH = dh;
        _lastInputV = dv;
    }

    int Move(int d, bool isH, float offsetTime)
    {
        var pos = transform.position;
        var isStart = isH
            ? Mathf.Approximately(pos.x, (int)pos.x)
            : Mathf.Approximately(pos.y, (int)pos.y);

        if (d == 0 && isStart) return 0;

        var apos = isH ? pos.x : pos.y;
        var undoing = d == 0;
        var dest = d == 1
            ? Mathf.CeilToInt(apos)
            : Mathf.FloorToInt(apos);

        if (
            isStart ||
            (
                (isH && d != _lastHDir) ||
                (!isH && d != _lastVDir)
            )
        )
        {
            var checkPadding = 1f - S.PlayerMoveUpdatePosAt;

            var minX = isH
                ? (int)pos.x - (d == -1 ? 1 : 0)
                : Mathf.FloorToInt(pos.x + (_lastHDir == 1 ? checkPadding : 0));
            var maxX = isH
                ? (int)pos.x + (d == 1 ? 1 : 0)
                : Mathf.CeilToInt(pos.x - (_lastHDir == -1 ? checkPadding : 0));
            var minY = !isH
                ? (int)pos.y - (d == -1 ? 1 : 0)
                : Mathf.FloorToInt(pos.y + (_lastVDir == 1 ? checkPadding : 0));
            var maxY = !isH
                ? (int)pos.y + (d == 1 ? 1 : 0)
                : Mathf.CeilToInt(pos.y - (_lastVDir == -1 ? checkPadding : 0));

            for (var col = minX; col <= maxX; ++col)
            {
                for (var row = minY; row <= maxY; ++row)
                {
                    var tile = new Vector2Int(col, row);
                    if (tile == _tilePos) continue;
                    if (TileController.Tiles[tile]?.Unwalkable() ?? false)
                    {
                        if (isStart) return 0;

                        undoing = true;
                        break;
                    }
                }

                if (undoing) break;
            }

            if (isStart) dest = (int)apos + d;
        }
        
        if (undoing)
        {
            var rounder = 0.5f - S.PlayerMoveDontUndoAt;
            dest = Mathf.RoundToInt(
                apos +
                ((isH ? _lastHDir : _lastVDir) * rounder)
            );
            d = (int)Mathf.Sign(dest - apos);
        }

        var dist = offsetTime / _moveTime;
        var passedDest = dist >= Mathf.Abs(
            isH ? pos.x - dest : pos.y - dest
        );

        if (passedDest)
        {
            transform.position = new Vector3(
                isH ? dest : pos.x,
                !isH ? dest : pos.y,
                pos.z
            );

            Pos = new Vector2Int(
                isH ? dest : _tilePos.x,
                !isH ? dest : _tilePos.y
            );

            if (
                (isH && d == _lastHDir) ||
                (!isH && d == _lastVDir)
            )
            {
                ++Stats.DistanceTravelled;

                if (!isH)
                {
                    var depth = -dest;
                    if (depth > _deepest)
                    {
                        Stats.DepthTravelled += depth - _deepest;
                        _deepest = depth;

                        if (depth > Stats.DeepestReached)
                        {
                            IsDeepest = true;
                            Stats.DeepestReached = depth;
                        }
                    }
                }
            }

            if (!undoing)
            {
                var travelled = Mathf.Abs(apos - dest);
                var timeUsed = travelled * _moveTime;

                Move(d, isH, offsetTime - timeUsed);
            }
        }
        else
            transform.position = new Vector3(
                isH ? pos.x + dist * d : pos.x,
                !isH ? pos.y + dist * d : pos.y,
                pos.z
            );

        return d;
    }
    
    public enum CauseOfGameOver
    {
        PlayerBomb,
        Mine,
        Time
    }
}