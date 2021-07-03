using System;
using System.Collections.Generic;
using MyLibrary;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Stats
{
#pragma warning disable CS0067
    public static event Action onStatsRefreshed;
#pragma warning restore CS0067

    static Dictionary<string, int> _intPrefs =
        new Dictionary<string, int>();
    
    static Dictionary<string, int> _intPrefOldValues =
        new Dictionary<string, int>();
    
    static Dictionary<string, float> _floatPrefs =
        new Dictionary<string, float>();

    public static int TotalScore
    {
        get => GetIntPref("totalScore");
        set => IncIntPrefTo("totalScore", value);
    }

    public static int HighestScore
    {
        get => GetIntPref("highestScore");
        set => SetIntPref("highestScore", value);
    }

    public static int DistanceTravelled
    {
        get => GetIntPref("distanceTravelled");
        set => IncIntPrefTo("distanceTravelled", value);
    }
    public static int DistanceTravelledDiff => GetIntPrefDiff("distanceTravelled");

    public static int DepthTravelled
    {
        get => GetIntPref("depthTravelled");
        set => IncIntPrefTo("depthTravelled", value);
    }
    public static int DepthTravelledDiff => GetIntPrefDiff("depthTravelled");

    public static int DeepestReached
    {
        get => GetIntPref("deepestReached");
        set => SetIntPref("deepestReached", value);
    }

    public static float TimePlayed
    {
        get => GetFloatPref("timePlayed");
        set => IncFloatPrefTo("timePlayed", value);
    }

    public static int GamesStarted
    {
        get => GetIntPref("gamesStarted");
        set => IncIntPrefTo("gamesStarted", value);
    }

    public static int LossesByTime
    {
        get => GetIntPref("lossesByTime");
        set => IncIntPrefTo("lossesByTime", value);
    }

    public static int LossesByOwnBomb
    {
        get => GetIntPref("lossesByOwnBomb");
        set => IncIntPrefTo("lossesByOwnBomb", value);
    }

    public static int LossesByMines
    {
        get => GetIntPref("lossesByMines");
        set => IncIntPrefTo("lossesByMines", value);
    }

    public static int ExtraLivesLost
    {
        get => GetIntPref("extraLivesLost");
        set => IncIntPrefTo("extraLivesLost", value);
    }
    public static int ExtraLivesLostDiff => GetIntPrefDiff("extraLivesLost");

    public static int BombsPlaced
    {
        get => GetIntPref("bombsPlaced");
        set => IncIntPrefTo("bombsPlaced", value);
    }
    public static int BombsPlacedDiff => GetIntPrefDiff("bombsPlaced");

    public static int LootCollected
    {
        get => GetIntPref("lootCollected");
        set => IncIntPrefTo("lootCollected", value);
    }
    public static int LootCollectedDiff => GetIntPrefDiff("lootCollected");

    public static int LootDestroyed
    {
        get => GetIntPref("lootDestroyed");
        set => IncIntPrefTo("lootDestroyed", value);
    }
    public static int LootDestroyedDiff => GetIntPrefDiff("lootDestroyed");

    public static int MinesDetonated
    {
        get => GetIntPref("minesDetonated");
        set => IncIntPrefTo("minesDetonated", value);
    }
    public static int MinesDetonatedDiff => GetIntPrefDiff("minesDetonated");

    public static int RocksDestroyed
    {
        get => GetIntPref("rocksDestroyed");
        set => IncIntPrefTo("rocksDestroyed", value);
    }
    public static int RocksDestroyedDiff => GetIntPrefDiff("rocksDestroyed");

    static Stats()
    {
        SceneManager.sceneUnloaded += _ =>
            _intPrefOldValues.Clear();
    
#if UNITY_IOS || CLOUDONCE_GOOGLE
        KVSCloudOnceSyncer.onSyncedFromCloudOnce += () =>
        {
            _intPrefs.Clear();
            _intPrefOldValues.Clear();
            _floatPrefs.Clear();

            onStatsRefreshed?.Invoke();
        };
#endif
    }

    public static void IncLootableCount(Lootable lootable) =>
        IncIntPref(LootablePref(lootable));

    public static int GetLootableCount(Lootable lootable) =>
        GetIntPref(LootablePref(lootable));
    
    public static int GetLootableCountDiff(Lootable lootable) =>
        GetIntPrefDiff(LootablePref(lootable));

    static int GetIntPref(string pref)
    {
        if (!_intPrefs.ContainsKey(pref))
            _intPrefs[pref] = KVSBridge.GetInt(pref, 0);

        return _intPrefs[pref];
    }

    static void SetIntPref(string pref, int value)
    {
        if (!_intPrefOldValues.ContainsKey(pref))
            _intPrefOldValues[pref] = GetIntPref(pref);

        KVS.SetInt(pref, value);
        _intPrefs[pref] = value;

        Kongregate.SetStat(pref, value);
    }

    static void IncIntPref(string pref, int amount = 1)
    {
        if (!_intPrefOldValues.ContainsKey(pref))
            _intPrefOldValues[pref] = GetIntPref(pref);

        var newVal = GetIntPref(pref) + amount;

        KVS.SetInt(pref, newVal);
        _intPrefs[pref] = newVal;

        Kongregate.SetStat(pref, amount);
    }
    
    static void IncIntPrefTo(string pref, int value)
    {
        if (!_intPrefOldValues.ContainsKey(pref))
            _intPrefOldValues[pref] = GetIntPref(pref);

        var inc = value - GetIntPref(pref);
        if (inc < 0) return;

        IncIntPref(pref, inc);
    }

    static int GetIntPrefDiff(string pref) =>
        _intPrefOldValues.ContainsKey(pref)
            ? GetIntPref(pref) - _intPrefOldValues[pref]
            : 0;

    static float GetFloatPref(string pref)
    {
        if (!_floatPrefs.ContainsKey(pref))
            _floatPrefs[pref] = KVSBridge.GetFloat(pref, 0f);

        return _floatPrefs[pref];
    }

    static void IncFloatPrefTo(string pref, float value)
    {
        if (!_floatPrefs.ContainsKey(pref))
            _floatPrefs[pref] = GetFloatPref(pref);

        var inc = value - GetFloatPref(pref);
        if (inc < 0) return;

        KVS.SetFloat(pref, value);
        _floatPrefs[pref] = value;

        Kongregate.SetStat(pref, Mathf.RoundToInt(inc));
    }

    static string LootablePref(Lootable lootable) =>
        "lootableCount_" + lootable.ToString();
}
