using System.Collections.Generic;
using UnityEngine;

/// <summary>A collection of statics providing a single point configuration.</summary>
public class S : MonoBehaviour
{
    public static IReadOnlyDictionary<Lootable, string>
        LootableDescriptions { get; private set; }

    static S _instance;


    #pragma warning disable CS0649
    public static float AudioMasterVolumeOn =>
        _instance._audioMasterVolumeOn;
    [SerializeField] float _audioMasterVolumeOn = -15f;
    public static float AudioVolumeOff =>
        _instance._audioVolumeOff;
    [SerializeField] float _audioVolumeOff = -80f;

    public static GameObject AvailableBombPrefab =>
        _instance._availableBombPrefab;
    [SerializeField] GameObject _availableBombPrefab;
    public static int AvailableBombLimit =>
        _instance._availableBombLimit;
    [SerializeField] int _availableBombLimit = 6;
    public static float AvailableBombInitialCooldown =>
        _instance._availableBombInitialCooldown;
    [SerializeField] float _availableBombInitialCooldown = 5f;
    public static float AvailableBombGrowShrinkTime =>
        _instance._availableBombGrowShrinkTime;
    [SerializeField] float _availableBombGrowShrinkTime = 0.5f;

    public static AnimationCurve DropExtraBombPowerChance =>
        _instance._dropExtraBombPowerChance;
    [SerializeField] AnimationCurve _dropExtraBombPowerChance;
    public static AnimationCurve DropExtraLifeChance =>
        _instance._dropExtraLifeChance;
    [SerializeField] AnimationCurve _dropExtraLifeChance;
    public static AnimationCurve DropExtraSpeedChance =>
        _instance._dropExtraSpeedChance;
    [SerializeField] AnimationCurve _dropExtraSpeedChance;
    public static AnimationCurve DropExtraTimeChance =>
        _instance._dropExtraTimeChance;
    [SerializeField] AnimationCurve _dropExtraTimeChance;
    public static AnimationCurve DropGoldenTimeChance =>
        _instance._dropGoldenTimeChance;
    [SerializeField] AnimationCurve _dropGoldenTimeChance;
    public static AnimationCurve DropMineChance =>
        _instance._dropMineChance;
    [SerializeField] AnimationCurve _dropMineChance;
    public static AnimationCurve DropMultibombChance =>
        _instance._dropMultibombChance;
    [SerializeField] AnimationCurve _dropMultibombChance;
    public static AnimationCurve DropBronzeChance =>
        _instance._dropBronzeChance;
    [SerializeField] AnimationCurve _dropBronzeChance;
    public static AnimationCurve DropSilverChance =>
        _instance._dropSilverChance;
    [SerializeField] AnimationCurve _dropSilverChance;
    public static AnimationCurve DropGoldChance =>
        _instance._dropGoldChance;
    [SerializeField] AnimationCurve _dropGoldChance;
    public static AnimationCurve DropPlatinumChance =>
        _instance._dropPlatinumChance;
    [SerializeField] AnimationCurve _dropPlatinumChance;
    public static AnimationCurve DropSapphireChance =>
        _instance._dropSapphireChance;
    [SerializeField] AnimationCurve _dropSapphireChance;
    public static AnimationCurve DropEmeraldChance =>
        _instance._dropEmeraldChance;
    [SerializeField] AnimationCurve _dropEmeraldChance;
    public static AnimationCurve DropRubyChance =>
        _instance._dropRubyChance;
    [SerializeField] AnimationCurve _dropRubyChance;
    public static AnimationCurve DropDiamondChance =>
        _instance._dropDiamondChance;
    [SerializeField] AnimationCurve _dropDiamondChance;
    public static AnimationCurve DropPinkDiamondChance =>
        _instance._dropPinkDiamondChance;
    [SerializeField] AnimationCurve _dropPinkDiamondChance;

    public static float ExplosionLifetime =>
        _instance._explosionLifetime;
    [SerializeField] float _explosionLifetime = 0.7f;
    public static float ExplosionEndBurnAt =>
        _instance._explosionEndBurnAt;
    [SerializeField] float _explosionEndBurnAt = 0.7f;
    public static float ExplosionSafeDistance =>
        _instance._explosionSafeDistance;
    [SerializeField] float _explosionSafeDistance = 0.3f;
    public static float ExplosionCombustAfter =>
        _instance._explosionCombustAfter;
    [SerializeField] float _explosionCombustAfter = 0.1f;

    public static GameObject FloorPiece =>
        _instance._floorPiecePrefab;
    [SerializeField] GameObject _floorPiecePrefab;
    public static Vector2 FloorUnitSize =>
        _instance._floorUnitSize;
    [SerializeField] Vector2 _floorUnitSize = new Vector2(32f, 32f);
    public static Vector2Int FloorNumCopies =>
        _instance._floorNumCopies;
    [SerializeField] Vector2Int _floorNumCopies = new Vector2Int(3, 3);

    public static float GameplayUIAnimTime =>
        _instance._gameplayUIAnimTime;
    [SerializeField] float _gameplayUIAnimTime = 1.5f;

    public static float MainCameraZoomTime =>
        _instance._mainCameraZoomTime;
    [SerializeField] float _mainCameraZoomTime = 3f;
    
    public static float MineExplodeAfter =>
        _instance._mineExplodeAfter;
    [SerializeField] float _mineExplodeAfter = 4f;
    public static float MineCombustAt =>
        _instance._mineCombustAt;
    [SerializeField] float _mineCombustAt = 0.975f;
    public static float MineDestroyAfter =>
        _instance._mineDestroyAfter;
    [SerializeField] float _mineDestroyAfter = 0.25f;
    public static float MinePlaceDurationMultiplier =>
        _instance._minePlacedDurationMultiplier;
    [SerializeField] float _minePlacedDurationMultiplier = 2f;
    public static bool MinePlacedHasReward =>
        _instance._minePlacedHasReward;
    [SerializeField] bool _minePlacedHasReward = false;

    public static GameObject MineExplosion =>
        _instance._mineExplosionPrefab;
    [SerializeField] GameObject _mineExplosionPrefab;
    public static int MineExplosionReach =>
        _instance._mineExplosionReach;
    [SerializeField] int _mineExplosionReach = 2;
    
    public static GameObject ObjectLootedParticle =>
        _instance._objectLootedParticle;
    [SerializeField] GameObject _objectLootedParticle;
    public static float ObjectLootedShrinkTime =>
        _instance._objectLootedShrinkTime;
    [SerializeField] float _objectLootedShrinkTime = 0.25f;

    public static AnimationCurve PlaceIndestructibleRockChance =>
        _instance._placeIndestructibleRockChance;
    [SerializeField] AnimationCurve _placeIndestructibleRockChance;
    public static AnimationCurve PlaceMineChance =>
        _instance._placeMineChance;
    [SerializeField] AnimationCurve _placeMineChance;
    public static AnimationCurve PlaceRockChance =>
        _instance._placeRockChance;
    [SerializeField] AnimationCurve _placeRockChance;

    public static GameObject PlayerAsh =>
        _instance._playerAshPrefab;
    [SerializeField] GameObject _playerAshPrefab;
    public static float PlayerBurnTime =>
        _instance._playerBurnTime;
    [SerializeField] float _playerBurnTime = 0.6f;

    public static float PlayerBombExplodeAfter =>
        _instance._playerBombExplodeAfter;
    [SerializeField] float _playerBombExplodeAfter = 2f;
    public static float PlayerBombCombustAt =>
        _instance._playerBombCombustAt;
    [SerializeField] float _playerBombCombustAt = 0.975f;
    public static float PlayerBombDestroyAfter =>
        _instance._playerBombDestroyAfter;
    [SerializeField] float _playerBombDestroyAfter = 0.25f;
    
    public static GameObject PlayerExplosion =>
        _instance._playerExplosionPrefab;
    [SerializeField] GameObject _playerExplosionPrefab;
    public static float PlayerExplosionFanOutTime =>
        _instance._playerExplosionFanOutTime;
    [SerializeField] float _playerExplosionFanOutTime = 0.15f;

    public static float PlayerMoveTime =>
        _instance._playerMoveTime;
    [SerializeField] float _playerMoveTime = 0.3f;
    public static float PlayerMoveUpdatePosAt =>
        _instance._playerMoveUpdatePosAt;
    [SerializeField] float _playerMoveUpdatePosAt = 0.8f;
    public static float PlayerMoveDontUndoAt =>
        _instance._playerMoveDontUndoAt;
    [SerializeField] float _playerMoveDontUndoAt = 0.35f;
    public static float PlayerMoveMaxRotPerSecond =>
        _instance._playerMoveMaxRotationPerSecond;
    [SerializeField] float _playerMoveMaxRotationPerSecond = 540f;

    public static int PowerupBombPowerLimit =>
        _instance._powerupBombPowerLimit;
    [SerializeField] int _powerupBombPowerLimit = 10;
    public static float PowerupExtraLifeSaveDuration =>
        _instance._powerupExtraLifeSaveDuration;
    [SerializeField] float _powerupExtraLifeSaveDuration = 1f;
    public static float PowerupExtraLifeCooldown =>
        _instance._powerupExtraLifeCooldown;
    [SerializeField] float _powerupExtraLifeCooldown = 40f;
    public static int PowerupExtraSpeedLimit =>
        _instance._powerupExtraSpeedLimit;
    [SerializeField] int _powerupExtraSpeedLimit = 5;
    public static float PowerupExtraSpeedDifference =>
        _instance._powerupExtraSpeedDifference;
    [SerializeField] float _powerupExtraSpeedDifference = 0.02f;
    public static float PowerupExtraTimeAmount =>
        _instance._powerupExtraTimeAmount;
    [SerializeField] float _powerupExtraTimeAmount = 20f;
    public static float PowerupGoldenTimeAmount =>
        _instance._powerupGoldenTimeAmount;
    [SerializeField] float _powerupGoldenTimeAmount = 50f;
    public static float PowerupExtraTimeCooldown =>
        _instance._powerupExtraTimeCooldown;
    [SerializeField] float _powerupExtraTimeCooldown = 5f;
    public static int PowerupMultibombLimit =>
        _instance._powerupMultibombLimit;
    [SerializeField] int _powerupMultibombLimit = 6;
    public static float PowerupMultibombDifference =>
        _instance._powerupMultibombDifference;
    [SerializeField] float _powerupMultibombDifference = 0.5f;
    
    public static int ScoreFromBronze =>
        _instance._scoreFromBronze;
    [SerializeField] int _scoreFromBronze = 1;
    public static int ScoreFromSilver =>
        _instance._scoreFromSilver;
    [SerializeField] int _scoreFromSilver = 2;
    public static int ScoreFromGold =>
        _instance._scoreFromGold;
    [SerializeField] int _scoreFromGold = 4;
    public static int ScoreFromPlatinum =>
        _instance._scoreFromPlatinum;
    [SerializeField] int _scoreFromPlatinum = 8;
    public static int ScoreFromSapphire =>
        _instance._scoreFromSapphire;
    [SerializeField] int _scoreFromSapphire = 16;
    public static int ScoreFromEmerald =>
        _instance._scoreFromEmerald;
    [SerializeField] int _scoreFromEmerald = 32;
    public static int ScoreFromRuby =>
        _instance._scoreFromRuby;
    [SerializeField] int _scoreFromRuby = 64;
    public static int ScoreFromDiamond =>
        _instance._scoreFromDiamond;
    [SerializeField] int _scoreFromDiamond = 128;
    public static int ScoreFromPinkDiamond =>
        _instance._scoreFromPinkDiamond;
    [SerializeField] int _scoreFromPinkDiamond = 256;
    
    public static Sprite SpriteLootableUnknown =>
        _instance._spriteLootableUnknown;
    [SerializeField] Sprite _spriteLootableUnknown;

    public static ExtraBombPower TileExtraBombPower =>
        _instance._tileExtraBombPowerPrefab;
    [SerializeField] ExtraBombPower _tileExtraBombPowerPrefab;
    public static ExtraLife TileExtraLife =>
        _instance._tileExtraLifePrefab;
    [SerializeField] ExtraLife _tileExtraLifePrefab;
    public static ExtraSpeed TileExtraSpeed =>
        _instance._tileExtraSpeedPrefab;
    [SerializeField] ExtraSpeed _tileExtraSpeedPrefab;
    public static ExtraTime TileExtraTime =>
        _instance._tileExtraTimePrefab;
    [SerializeField] ExtraTime _tileExtraTimePrefab;
    public static GoldenTime TileGoldenTime =>
        _instance._tileGoldenTimePrefab;
    [SerializeField] GoldenTime _tileGoldenTimePrefab;
    public static Mine TileMine =>
        _instance._tileMinePrefab;
    [SerializeField] Mine _tileMinePrefab;
    public static Multibomb TileMultibomb =>
        _instance._tileMultibombPrefab;
    [SerializeField] Multibomb _tileMultibombPrefab;
    public static Bronze TileBronze =>
        _instance._tileBronzePrefab;
    [SerializeField] Bronze _tileBronzePrefab;
    public static Diamond TileDiamond =>
        _instance._tileDiamondPrefab;
    [SerializeField] Diamond _tileDiamondPrefab;
    public static Emerald TileEmerald =>
        _instance._tileEmeraldPrefab;
    [SerializeField] Emerald _tileEmeraldPrefab;
    public static Gold TileGold =>
        _instance._tileGoldPrefab;
    [SerializeField] Gold _tileGoldPrefab;
    public static PinkDiamond TilePinkDiamond =>
        _instance._tilePinkDiamondPrefab;
    [SerializeField] PinkDiamond _tilePinkDiamondPrefab;
    public static Platinum TilePlatinum =>
        _instance._tilePlatinumPrefab;
    [SerializeField] Platinum _tilePlatinumPrefab;
    public static Ruby TileRuby =>
        _instance._tileRubyPrefab;
    [SerializeField] Ruby _tileRubyPrefab;
    public static Sapphire TileSapphire =>
        _instance._tileSapphirePrefab;
    [SerializeField] Sapphire _tileSapphirePrefab;
    public static Silver TileSilver =>
        _instance._tileSilverPrefab;
    [SerializeField] Silver _tileSilverPrefab;
    public static IndestructibleRock TileIndestructibleRock =>
        _instance._tileIndestructibleRockPrefab;
    [SerializeField] IndestructibleRock _tileIndestructibleRockPrefab;
    public static PlayerBomb TilePlayerBomb =>
        _instance._tilePlayerBombPrefab;
    [SerializeField] PlayerBomb _tilePlayerBombPrefab;
    public static Rock TileRock =>
        _instance._tileRockPrefab;
    [SerializeField] Rock _tileRockPrefab;
    
    public static int TilesSpawnDistanceX =>
        _instance._tilesSpawnDistanceX;
    [SerializeField] int _tilesSpawnDistanceX = 14;
    public static int TilesSpawnDistanceY =>
        _instance._tilesSpawnDistanceY;
    [SerializeField] int _tilesSpawnDistanceY = 10;

    public static float TimerStartingTime =>
        _instance._timerStartingTime;
    [SerializeField] float _timerStartingTime = 93f;
    

    [SerializeField] int _gameTargetFrameRate = 60;
    #pragma warning restore CS0649

    void Awake()
    {
        _instance = this;
        Application.targetFrameRate = _gameTargetFrameRate;

        LootableDescriptions = new Dictionary<Lootable, string>
        {   { Lootable.Bronze, string.Format("+{0} score", ScoreFromBronze) }
        ,   { Lootable.Diamond, string.Format("+{0} score", ScoreFromDiamond) }
        ,   { Lootable.Emerald, string.Format("+{0} score", ScoreFromEmerald) }
        ,   { Lootable.ExtraBombPower, "Increases your bombs' explosion distance. When at max, allows explosions to pierce through a rock" }
        ,   { Lootable.ExtraLife, "Saves you from death once. Can only carry one at a time" }
        ,   { Lootable.ExtraSpeed, "Makes you move faster" }
        ,   { Lootable.ExtraTime, string.Format("+{0} seconds to timer", PowerupExtraTimeAmount) }
        ,   { Lootable.GoldenTime, string.Format("+{0} seconds to timer", PowerupGoldenTimeAmount) }
        ,   { Lootable.Gold, string.Format("+{0} score", ScoreFromGold) }
        ,   { Lootable.Multibomb, "Makes your bombs recharge faster" }
        ,   { Lootable.PinkDiamond, string.Format("+{0} score", ScoreFromPinkDiamond) }
        ,   { Lootable.Platinum, string.Format("+{0} score", ScoreFromPlatinum) }
        ,   { Lootable.Ruby, string.Format("+{0} score", ScoreFromRuby) }
        ,   { Lootable.Sapphire, string.Format("+{0} score", ScoreFromSapphire) }
        ,   { Lootable.Silver, string.Format("+{0} score", ScoreFromSilver) }
        };
    }
}

public enum Lootable
{
    Bronze = 0,
    Diamond = 1,
    Emerald = 2,
    ExtraBombPower = 3,
    ExtraLife = 4,
    ExtraSpeed = 5,
    ExtraTime = 6,
    Gold = 7,
    GoldenTime = 14,
    Multibomb = 8,
    PinkDiamond = 9,
    Platinum = 10,
    Ruby = 11,
    Sapphire = 12,
    Silver = 13
}