using UnityEngine;

/// <summary>A collection of statics providing a single point configuration.</summary>
public class S : MonoBehaviour
{
    public static float AdFirstAfter =>
        _instance._adFirstAfter;
    public static float AdMinTimeBetween =>
        _instance._adMinTimeBetween;
    public static float AdDelay =>
        _instance._adDelay;
    public static float AdCheckPeriod =>
        _instance._adCheckPeriod;
    public static float AdGiveUpAfter =>
        _instance._adGiveUpAfter;

    public static float AudioMasterVolumeOn =>
        _instance._audioMasterVolumeOn;
    public static float AudioVolumeOff =>
        _instance._audioVolumeOff;

    public static GameObject AvailableBombPrefab =>
        _instance._availableBombPrefab;
    public static int AvailableBombLimit =>
        _instance._availableBombLimit;
    public static float AvailableBombInitialCooldown =>
        _instance._availableBombInitialCooldown;
    public static float AvailableBombGrowShrinkTime =>
        _instance._availableBombGrowShrinkTime;

    public static AnimationCurve DropExtraBombPowerChance =>
        _instance._dropExtraBombPowerChance;
    public static AnimationCurve DropExtraLifeChance =>
        _instance._dropExtraLifeChance;
    public static AnimationCurve DropExtraSpeedChance =>
        _instance._dropExtraSpeedChance;
    public static AnimationCurve DropExtraTimeChance =>
        _instance._dropExtraTimeChance;
    public static AnimationCurve DropMultibombChance =>
        _instance._dropMultibombChance;
    public static AnimationCurve DropMineChance =>
        _instance._dropMineChance;
    public static AnimationCurve DropGoldChance =>
        _instance._dropGoldChance;
    public static AnimationCurve DropEmeraldChance =>
        _instance._dropEmeraldChance;
    public static AnimationCurve DropDiamondChance =>
        _instance._dropDiamondChance;

    public static float ExplosionLifetime =>
        _instance._explosionLifetime;
    public static float ExplosionEndBurnAt =>
        _instance._explosionEndBurnAt;
    public static float ExplosionSafeDistance =>
        _instance._explosionSafeDistance;
    public static float ExplosionCombustAfter =>
        _instance._explosionCombustAfter;

    public static GameObject FloorPiece =>
        _instance._floorPiecePrefab;
    public static Vector2 FloorUnitSize =>
        _instance._floorUnitSize;
    public static Vector2Int FloorNumCopies =>
        _instance._floorNumCopies;

    public static float GameplayUIAnimTime =>
        _instance._gameplayUIAnimTime;

    public static float MainCameraZoomTime =>
        _instance._mainCameraZoomTime;
    
    public static float MineExplodeAfter =>
        _instance._mineExplodeAfter;
    public static float MineCombustAt =>
        _instance._mineCombustAt;
    public static float MineDestroyAfter =>
        _instance._mineDestroyAfter;
    public static float MinePlaceDurationMultiplier =>
        _instance._minePlacedDurationMultiplier;

    public static GameObject MineExplosion =>
        _instance._mineExplosionPrefab;
    public static int MineExplosionReach =>
        _instance._mineExplosionReach;
    
    public static GameObject ObjectLootedParticle =>
        _instance._objectLootedParticle;
    public static float ObjectLootedShrinkTime =>
        _instance._objectLootedShrinkTime;

    public static AnimationCurve PlaceExtraBombPowerChance =>
        _instance._placeExtraBombPowerChance;
    public static AnimationCurve PlaceExtraLifeChance =>
        _instance._placeExtraLifeChance;
    public static AnimationCurve PlaceExtraSpeedChance =>
        _instance._placeExtraSpeedChance;
    public static AnimationCurve PlaceExtraTimeChance =>
        _instance._placeExtraTimeChance;
    public static AnimationCurve PlaceMultibombChance =>
        _instance._placeMultibombChance;
    public static AnimationCurve PlaceIndestructibleRockChance =>
        _instance._placeIndestructibleRockChance;
    public static AnimationCurve PlaceRockChance =>
        _instance._placeRockChance;
    public static AnimationCurve PlaceMineChance =>
        _instance._placeMineChance;

    public static float PlayerMoveTime =>
        _instance._playerMoveTime;
    public static float PlayerMaxRotPerSecond =>
        _instance._playerMaxRotationPerSecond;
    public static GameObject PlayerAsh =>
        _instance._playerAshPrefab;
    public static float PlayerBurnTime =>
        _instance._playerBurnTime;

    public static float PlayerBombExplodeAfter =>
        _instance._playerBombExplodeAfter;
    public static float PlayerBombCombustAt =>
        _instance._playerBombCombustAt;
    public static float PlayerBombDestroyAfter =>
        _instance._playerBombDestroyAfter;
    
    public static GameObject PlayerExplosion =>
        _instance._playerExplosionPrefab;
    public static float PlayerExplosionFanOutTime =>
        _instance._playerExplosionFanOutTime;
    public static float PlayerExplosionPierceRatio =>
        _instance._playerExplosionPierceRatio;

    public static float PowerupLifetime =>
        _instance._powerupLifetime;
    public static int PowerupBombPowerLimit =>
        _instance._powerupBombPowerLimit;
    public static float PowerupExtraLifeSaveDuration =>
        _instance._powerupExtraLifeSaveDuration;
    public static float PowerupExtraLifeCooldown =>
        _instance._powerupExtraLifeCooldown;
    public static int PowerupExtraSpeedLimit =>
        _instance._powerupExtraSpeedLimit;
    public static float PowerupExtraSpeedDifference =>
        _instance._powerupExtraSpeedDifference;
    public static float PowerupExtraTimeAmount =>
        _instance._powerupExtraTimeAmount;
    public static float PowerupExtraTimeCooldown =>
        _instance._powerupExtraTimeCooldown;
    public static int PowerupMultibombLimit =>
        _instance._powerupMultibombLimit;
    public static float PowerupMultibombDifference =>
        _instance._powerupMultibombDifference;
    
    public static int ScoreFromGold =>
        _instance._scoreFromGold;
    public static int ScoreFromEmerald =>
        _instance._scoreFromEmerald;
    public static int ScoreFromDiamond =>
        _instance._scoreFromDiamond;

    public static ExtraBombPower TileExtraBombPower =>
        _instance._tileExtraBombPowerPrefab;
    public static ExtraLife TileExtraLife =>
        _instance._tileExtraLifePrefab;
    public static ExtraSpeed TileExtraSpeed =>
        _instance._tileExtraSpeedPrefab;
    public static ExtraTime TileExtraTime =>
        _instance._tileExtraTimePrefab;
    public static Multibomb TileMultibomb =>
        _instance._tileMultibombPrefab;
    public static Diamond TileDiamond =>
        _instance._tileDiamondPrefab;
    public static Emerald TileEmerald =>
        _instance._tileEmeraldPrefab;
    public static Gold TileGold =>
        _instance._tileGoldPrefab;
    public static Mine TileMine =>
        _instance._tileMinePrefab;
    public static IndestructibleRock TileIndestructibleRock =>
        _instance._tileIndestructibleRockPrefab;
    public static PlayerBomb TilePlayerBomb =>
        _instance._tilePlayerBombPrefab;
    public static Rock TileRock =>
        _instance._tileRockPrefab;
    
    public static int TilesSpawnDistanceX =>
        _instance._tilesSpawnDistanceX;
    public static int TilesSpawnDistanceY =>
        _instance._tilesSpawnDistanceY;

    public static float TimerStartingTime =>
        _instance._timerStartingTime;
    
    static S _instance;

    #pragma warning disable CS0649
    [SerializeField]
    float _adFirstAfter = 120f;
    [SerializeField]
    float _adMinTimeBetween = 240f;
    [SerializeField]
    float _adDelay = 1.5f;
    [SerializeField]
    float _adCheckPeriod = 0.1f;
    [SerializeField]
    float _adGiveUpAfter = 5f;

    [SerializeField]
    float _audioMasterVolumeOn = -15f;
    [SerializeField]
    float _audioVolumeOff = -80f;

    [SerializeField]
    GameObject _availableBombPrefab;
    [SerializeField]
    int _availableBombLimit = 6;
    [SerializeField]
    float _availableBombInitialCooldown = 5f;
    [SerializeField]
    float _availableBombGrowShrinkTime = 0.5f;

    [SerializeField]
    AnimationCurve _dropExtraBombPowerChance;
    [SerializeField]
    AnimationCurve _dropExtraLifeChance;
    [SerializeField]
    AnimationCurve _dropExtraSpeedChance;
    [SerializeField]
    AnimationCurve _dropExtraTimeChance;
    [SerializeField]
    AnimationCurve _dropMultibombChance;
    [SerializeField]
    AnimationCurve _dropGoldChance;
    [SerializeField]
    AnimationCurve _dropEmeraldChance;
    [SerializeField]
    AnimationCurve _dropDiamondChance;
    [SerializeField]
    AnimationCurve _dropMineChance;

    [SerializeField]
    float _explosionLifetime = 0.7f;
    [SerializeField]
    float _explosionEndBurnAt = 0.7f;
    [SerializeField]
    float _explosionSafeDistance = 0.3f;
    [SerializeField]
    float _explosionCombustAfter = 0.1f;

    [SerializeField]
    GameObject _floorPiecePrefab;
    [SerializeField]
    Vector2 _floorUnitSize = new Vector2(32f, 32f);
    [SerializeField]
    Vector2Int _floorNumCopies = new Vector2Int(3, 3);

    [SerializeField]
    int _gameTargetFrameRate = 60;

    [SerializeField]
    float _gameplayUIAnimTime = 1.5f;

    [SerializeField]
    float _mainCameraZoomTime = 3f;

    [SerializeField]
    float _mineExplodeAfter = 4f;
    [SerializeField]
    float _mineCombustAt = 0.975f;
    [SerializeField]
    float _mineDestroyAfter = 0.25f;
    [SerializeField]
    float _minePlacedDurationMultiplier = 2f;

    [SerializeField]
    GameObject _mineExplosionPrefab;
    [SerializeField]
    int _mineExplosionReach = 2;

    [SerializeField]
    GameObject _objectLootedParticle;
    [SerializeField]
    float _objectLootedShrinkTime = 0.25f;

    [SerializeField]
    AnimationCurve _placeExtraBombPowerChance;
    [SerializeField]
    AnimationCurve _placeExtraLifeChance;
    [SerializeField]
    AnimationCurve _placeExtraSpeedChance;
    [SerializeField]
    AnimationCurve _placeExtraTimeChance;
    [SerializeField]
    AnimationCurve _placeMultibombChance;
    [SerializeField]
    AnimationCurve _placeIndestructibleRockChance;
    [SerializeField]
    AnimationCurve _placeRockChance;
    [SerializeField]
    AnimationCurve _placeMineChance;
    
    [SerializeField]
    float _playerMoveTime = 0.3f;
    [SerializeField]
    float _playerMaxRotationPerSecond = 480f;
    [SerializeField]
    GameObject _playerAshPrefab;
    [SerializeField]
    float _playerBurnTime = 0.6f;

    [SerializeField]
    float _playerBombExplodeAfter = 2f;
    [SerializeField]
    float _playerBombCombustAt = 0.975f;
    [SerializeField]
    float _playerBombDestroyAfter = 0.25f;

    [SerializeField]
    GameObject _playerExplosionPrefab;
    [SerializeField]
    float _playerExplosionFanOutTime = 0.15f;
    [SerializeField]
    float _playerExplosionPierceRatio = 0.2f;

    [SerializeField]
    float _powerupLifetime = 6f;
    [SerializeField]
    int _powerupBombPowerLimit = 10;
    [SerializeField]
    float _powerupExtraLifeSaveDuration = 1f;
    [SerializeField]
    float _powerupExtraLifeCooldown = 40f;
    [SerializeField]
    int _powerupExtraSpeedLimit = 5;
    [SerializeField]
    float _powerupExtraSpeedDifference = 0.025f;
    [SerializeField]
    float _powerupExtraTimeAmount = 20f;
    [SerializeField]
    float _powerupExtraTimeCooldown = 5f;
    [SerializeField]
    int _powerupMultibombLimit = 6;
    [SerializeField]
    float _powerupMultibombDifference = 0.6f;

    [SerializeField]
    int _scoreFromGold = 1;
    [SerializeField]
    int _scoreFromEmerald = 4;
    [SerializeField]
    int _scoreFromDiamond = 7;

    [SerializeField]
    ExtraBombPower _tileExtraBombPowerPrefab;
    [SerializeField]
    ExtraLife _tileExtraLifePrefab;
    [SerializeField]
    ExtraSpeed _tileExtraSpeedPrefab;
    [SerializeField]
    ExtraTime _tileExtraTimePrefab;
    [SerializeField]
    Multibomb _tileMultibombPrefab;
    [SerializeField]
    Diamond _tileDiamondPrefab;
    [SerializeField]
    Emerald _tileEmeraldPrefab;
    [SerializeField]
    Gold _tileGoldPrefab;
    [SerializeField]
    Mine _tileMinePrefab;
    [SerializeField]
    IndestructibleRock _tileIndestructibleRockPrefab;
    [SerializeField]
    PlayerBomb _tilePlayerBombPrefab;
    [SerializeField]
    Rock _tileRockPrefab;
    
    [SerializeField]
    int _tilesSpawnDistanceX = 14;
    [SerializeField]
    int _tilesSpawnDistanceY = 10;

    [SerializeField]
    float _timerStartingTime = 93f;
    #pragma warning restore CS0649

    void Awake()
    {
        _instance = this;
        Application.targetFrameRate = _gameTargetFrameRate;
    }
}