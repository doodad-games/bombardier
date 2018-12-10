using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public static bool GameOver { get; private set; }

    static bool _hasExtraLife;
    static Action<bool> _extraLifeChanged;
    static int _bombPower;
    static int _extraSpeed;
    static float _moveTime;
    static float _maxRotPerSecond;
    static int _multibomb;
    static Action<int> _multibombChanged;
    static Action<CauseOfGameOver> _gameOver;
    static Action<Vector2Int, Vector2Int> _moving;
    
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
            _extraLifeChanged?.Invoke(value);
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
            _maxRotPerSecond = S.PlayerMaxRotPerSecond *
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

            _multibombChanged?.Invoke(value);
        }
    }

    public static Vector2Int PreMovePos =>
        Instance._preMovePos;

    public static Vector2Int? ClosestEmptyTile =>
        Instance._ClosestEmptyTile();
    
    static Player() =>
        Timer.SubElapsed(() =>
        {
            Instance.EndGame(CauseOfGameOver.Time);
            Sounds.TimeOut.Play();
        });
    
    public static void SubExtraLife(Action<bool> cb) =>
        _extraLifeChanged += cb;
    public static void UnsubExtraLife(Action<bool> cb) =>
        _extraLifeChanged -= cb;
    
    public static void SubMultibomb(Action<int> cb) =>
        _multibombChanged += cb;
    public static void UnsubMultibomb(Action<int> cb) =>
        _multibombChanged -= cb;
        
    public static void SubGameOver(Action<CauseOfGameOver> cb) =>
        _gameOver += cb;
    public static void UnsubGameOver(Action<CauseOfGameOver> cb) =>
        _gameOver -= cb;

    public static void SubMove(Action<Vector2Int, Vector2Int> cb) =>
        _moving += cb;
    public static void UnsubMove(Action<Vector2Int, Vector2Int> cb) =>
        _moving -= cb;

    static IReadOnlyDictionary<Vector3, float> ZRotationForMovement =
        new Dictionary<Vector3, float>
        {   { new Vector3(1f, 0f, 0f), 270f }
        ,   { new Vector3(1f, 1f, 0f), 315f }
        ,   { new Vector3(0f, 1f, 0f), 0f }
        ,   { new Vector3(-1f, 1f, 0f), 45f }
        ,   { new Vector3(-1f, 0f, 0f), 90f }
        ,   { new Vector3(-1f, -1f, 0f), 135f }
        ,   { new Vector3(0f, -1f, 0f), 180f }
        ,   { new Vector3(1f, -1f, 0f), 225f }
        };
    
    #pragma warning disable CS0649
    [SerializeField]
    Transform _camera;
    [SerializeField]
    Animator _animator;
    [SerializeField]
    Transform _toRotate;
    #pragma warning restore CS0649
    
    Vector2Int _preMovePos;
    Vector2Int _postMovePos;
    Vector3 _lastRotation;
    int? _movingH;
    int? _movingV;
    bool _rotating;
    Quaternion _rotateTo;
    float _safeUntil;

    public void Burn(bool isMine)
    {
        if (GameOver || Time.time < _safeUntil) { return; }

        if (_hasExtraLife)
        {
            HasExtraLife = false;

            _safeUntil = Time.time + S.PowerupExtraLifeSaveDuration;

            Sounds.LoseLife.Play();
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
        Sounds.PlayerBurn.Play();

        EndGame(
            isMine
                ? CauseOfGameOver.Mine
                : CauseOfGameOver.PlayerBomb
        );
    }

    void Awake()
    {
        Time.timeScale = 1f;
        Instance = this;
        GameOver = false;
    }

    void Start()
    {
        BombPower = 1;
        ExtraSpeed = 0;
        _multibomb = 0;

        _animator.SetFloat("BurnPlaybackSpeed", 1 / S.PlayerBurnTime);

        var curPos = transform.position;
        _preMovePos = _postMovePos = new Vector2Int(
            (int)curPos.x,
            (int)curPos.y
        );

        _lastRotation = new Vector3(0f, 1f, 0f);
    }

    void Update() =>
        Update(0f);
    void Update(float offsetTime)
    {
        var h = _movingH == null
            ? Joystick.Horizontal + Input.GetAxis("Horizontal")
            : 0f;
        var roundH = h == 0f ? 0 : Mathf.RoundToInt(h);
        
        var v = _movingV == null
            ? Joystick.Vertical + Input.GetAxis("Vertical")
            : 0f;
        var roundV = v == 0f ? 0 : Mathf.RoundToInt(v);

        var hFirst = roundH != 0f && Mathf.Abs(h) > Mathf.Abs(v);
        if (hFirst)
        {
            if (roundH != 0f) { StartCoroutine(Move(roundH, offsetTime, true)); }
            if (roundV != 0f) { StartCoroutine(Move(roundV, offsetTime, false)); }
        }
        else
        {
            if (roundV != 0f) { StartCoroutine(Move(roundV, offsetTime, false)); }
            if (roundH != 0f) { StartCoroutine(Move(roundH, offsetTime, true)); }
        }

        _animator.SetBool(
            "Moving",
            !(_movingH == null && _movingV == null)
        );

        if (_movingH == null && _movingV == null)
        {
            Sounds.Movement.Pause();
        }
        else if (!Sounds.Movement.isPlaying)
        {
            Sounds.Movement.Play();
        }
    }

    void EndGame(CauseOfGameOver cause)
    {
        if (GameOver) { return; }

        Time.timeScale = 0.5f;
        _animator.SetFloat("MovePlaybackSpeed", 0f);
        GameOver = true;
        _gameOver?.Invoke(cause);

        enabled = false;
        Sounds.Movement.Pause();
    }

    IEnumerator Move(int d, float offsetTime, bool isH)
    {
        var before = _postMovePos;
        var after = before + new Vector2Int(
            isH ? d : 0,
            !isH ? d : 0
        );

        {
            var minX = Mathf.Min(_preMovePos.x, after.x);
            var maxX = Mathf.Max(_preMovePos.x, after.x);
            var minY = Mathf.Min(_preMovePos.y, after.y);
            var maxY = Mathf.Max(_preMovePos.y, after.y);

            for (var col = minX; col <= maxX; ++col)
            {
                for (var row = minY; row <= maxY; ++row)
                {
                    var pos = new Vector2Int(col, row);
                    if (pos == _preMovePos) { continue; }

                    if (TileController.Tiles[pos]?.Unwalkable() ?? false)
                    {
                        yield break;
                    }
                }
            }
        }

        if (isH) { _movingH = d; }
        else { _movingV = d; }

        UpdateRotation();

        var amountMoved = offsetTime;
        var startTime = Time.time;

        _postMovePos = after;
        _moving?.Invoke(before, after);
    
        while (true)
        {
            if (GameOver) { yield break; }
            if (_movingH == null && _movingV == null) { break; }

            var newAmountMoved =
                d * Mathf.Min(
                    1f,
                    (Time.time - startTime) / _moveTime
                );
            
            var cur = isH
                ? transform.position.x
                : transform.position.y;
            
            var newAxisVal = cur - amountMoved + newAmountMoved;

            var pos = transform.position;
            if (isH) { pos.x = newAxisVal; }
            else { pos.y = newAxisVal; }
            transform.position = pos;

            amountMoved = newAmountMoved;

            if (amountMoved == d) { break; }

            yield return new WaitForEndOfFrame();
        }

        if (isH) { _movingH = null; }
        else { _movingV = null; }

        _preMovePos = after;
        
        if (_movingH == null && _movingV == null)
        {
            var pos = transform.position;
            transform.position = new Vector3(
                Mathf.Round(pos.x),
                Mathf.Round(pos.y),
                transform.position.z
            );
        }

        Update((Time.time - startTime) - _moveTime);

        yield return new WaitForSeconds(_moveTime * 0.15f);

        if (!(_movingH == null && _movingV == null))
        {
            UpdateRotation(true);
        }
    }

    void UpdateRotation(bool angleOnly = false)
    {
        var newRotation = new Vector3(
            _movingH ?? 0f,
            _movingV ?? 0f,
            0f
        );

        if (
            newRotation == Vector3.zero ||
            newRotation == _lastRotation
        ) { return; }

        _rotateTo = Quaternion.Euler(0f, 0f, ZRotationForMovement[newRotation]);

        if (!_rotating) { StartCoroutine(Rotate()); }

        _lastRotation = newRotation;
    }

    IEnumerator Rotate()
    {
        _rotating = true;
        
        while (true)
        {
            if (GameOver) { yield break; }

            _toRotate.rotation = Quaternion.RotateTowards(
                _toRotate.rotation,
                _rotateTo,
                _maxRotPerSecond * Time.deltaTime
            );

            if (_toRotate.rotation == _rotateTo) { break; }

            yield return new WaitForEndOfFrame();
        }

        _rotating = false;
    }
    
    Vector2Int? _ClosestEmptyTile()
    {
        var playerPos = transform.position;

        var minCol = Mathf.Min(_preMovePos.x, _postMovePos.x);
        var maxCol = Mathf.Max(_preMovePos.x, _postMovePos.x);
        var numCols = maxCol - minCol + 1;
        var minRow = Mathf.Min(_preMovePos.y, _postMovePos.y);
        var maxRow = Mathf.Max(_preMovePos.y, _postMovePos.y);
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
                            playerPos
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

    public enum CauseOfGameOver
    {
        PlayerBomb,
        Mine,
        Time
    }
}