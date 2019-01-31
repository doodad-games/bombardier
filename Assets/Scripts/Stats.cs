using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Stats
{
    static Dictionary<string, int> _intPrefs =
        new Dictionary<string, int>();
    
    static Dictionary<string, int> _intPrefOldValues =
        new Dictionary<string, int>();
    
    static Dictionary<string, float> _floatPrefs =
        new Dictionary<string, float>();

    public static int TotalScore
    {
        get => GetIntPref("totalScore");
        set => SetIntPref("totalScore", value);
    }

    public static int HighestScore
    {
        get => GetIntPref("highestScore");
        set => SetIntPref("highestScore", value);
    }

    public static int DistanceTravelled
    {
        get => GetIntPref("distanceTravelled");
        set => SetIntPref("distanceTravelled", value);
    }
    public static int DistanceTravelledDiff => GetIntPrefDiff("distanceTravelled");

    public static int DepthTravelled
    {
        get => GetIntPref("depthTravelled");
        set => SetIntPref("depthTravelled", value);
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
        set => SetFloatPref("timePlayed", value);
    }

    public static int GamesStarted
    {
        get => GetIntPref("gamesStarted");
        set => SetIntPref("gamesStarted", value);
    }

    public static int LossesByTime
    {
        get => GetIntPref("lossesByTime");
        set => SetIntPref("lossesByTime", value);
    }

    public static int LossesByOwnBomb
    {
        get => GetIntPref("lossesByOwnBomb");
        set => SetIntPref("lossesByOwnBomb", value);
    }

    public static int ExtraLivesLost
    {
        get => GetIntPref("extraLivesLost");
        set => SetIntPref("extraLivesLost", value);
    }
    public static int ExtraLivesLostDiff => GetIntPrefDiff("extraLivesLost");

    public static int BombsPlaced
    {
        get => GetIntPref("bombsPlaced");
        set => SetIntPref("bombsPlaced", value);
    }
    public static int BombsPlacedDiff => GetIntPrefDiff("bombsPlaced");

    public static int LootCollected
    {
        get => GetIntPref("lootCollected");
        set => SetIntPref("lootCollected", value);
    }
    public static int LootCollectedDiff => GetIntPrefDiff("lootCollected");

    public static int LootDestroyed
    {
        get => GetIntPref("lootDestroyed");
        set => SetIntPref("lootDestroyed", value);
    }
    public static int LootDestroyedDiff => GetIntPrefDiff("lootDestroyed");

    public static int RocksDestroyed
    {
        get => GetIntPref("rocksDestroyed");
        set => SetIntPref("rocksDestroyed", value);
    }
    public static int RocksDestroyedDiff => GetIntPrefDiff("rocksDestroyed");

    static Stats() =>
        SceneManager.sceneUnloaded += _ =>
            _intPrefOldValues.Clear();
    public static void IncLootableCount(Lootable lootable)
    {
        var pref = LootablePref(lootable);

        SetIntPref(pref, GetIntPref(pref) + 1);
    }

    public static int GetLootableCount(Lootable lootable) =>
        GetIntPref(LootablePref(lootable));
    
    public static int GetLootableCountDiff(Lootable lootable) =>
        GetIntPrefDiff(LootablePref(lootable));

    static int GetIntPref(string pref)
    {
        if (!_intPrefs.ContainsKey(pref))
            _intPrefs[pref] = PlayerPrefs.GetInt(pref, 0);

        return _intPrefs[pref];
    }

    static void SetIntPref(string pref, int value)
    {
        if (!_intPrefOldValues.ContainsKey(pref))
            _intPrefOldValues[pref] = GetIntPref(pref);

        PlayerPrefs.SetInt(pref, value);
        _intPrefs[pref] = value;
    }

    static int GetIntPrefDiff(string pref) =>
        _intPrefOldValues.ContainsKey(pref)
            ? GetIntPref(pref) - _intPrefOldValues[pref]
            : 0;

    static float GetFloatPref(string pref)
    {
        if (!_floatPrefs.ContainsKey(pref))
            _floatPrefs[pref] = PlayerPrefs.GetFloat(pref, 0f);

        return _floatPrefs[pref];
    }

    static void SetFloatPref(string pref, float value)
    {
        PlayerPrefs.SetFloat(pref, value);
        _floatPrefs[pref] = value;
    }

    static string LootablePref(Lootable lootable) =>
        "lootableCount_" + lootable.ToString();
}
