using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    const float TimeSavePeriod = 5f;

    public static float TimePassed { get; private set; }

    public static float TimeRemaining;
    public static event Action onElapsed;

    static Timer _instance;
    static bool _stopped;

    static Timer()
    {
        Player.onGameOver += (c) =>
        {
            _stopped = true;
            Stats.TimePlayed = _instance._timeBeforePlay + TimePassed;
        };

        GameplayUI.onMenuHidden += () => _stopped = false;
    }
    
    #pragma warning disable CS0649
    [SerializeField]
    TextMeshProUGUI _text;
    #pragma warning restore CS0649

    float _timeBeforePlay;
    float _nextSaveTime;

    void Awake()
    {
        _instance = this;
        TimePassed = 0f;
    }

    void Start()
    {
        TimeRemaining = S.TimerStartingTime;
        _stopped = GameplayUI.MenuShowing;

        _timeBeforePlay = Stats.TimePlayed;
        _nextSaveTime = Time.time + TimeSavePeriod;
    }
    
    void Update()
    {
        if (_stopped) { return; }

        TimeRemaining -= Time.deltaTime;
        TimePassed += Time.deltaTime;

        if (Time.time > _nextSaveTime)
        {
            _nextSaveTime += TimeSavePeriod;
            Stats.TimePlayed = _timeBeforePlay + TimePassed;
        }

        _text.text = TimeRemaining.ToString("0");

        if (TimeRemaining <= 0f)
        {
            ++Stats.LossesByTime;
            _stopped = true;
            onElapsed?.Invoke();
        }
    }
}