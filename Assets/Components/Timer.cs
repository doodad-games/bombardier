using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static float TimeRemaining;

    static Action _elapsed;
    static bool _stopped;

    static Timer() =>
        Player.SubGameOver((c) => _stopped = true);
    
    public static void SubElapsed(Action cb) =>
        _elapsed += cb;
    public static void UnsubElapsed(Action cb) =>
        _elapsed -= cb;

    #pragma warning disable CS0649
    [SerializeField]
    Text _text;
    #pragma warning restore CS0649

    void Awake() =>
        _stopped = false;

    void Start() =>
        TimeRemaining = S.TimerStartingTime;
    
    void Update()
    {
        if (_stopped) { return; }

        TimeRemaining -= Time.deltaTime;
        _text.text = TimeRemaining.ToString("0");

        if (TimeRemaining <= 0f)
        {
            _stopped = true;
            _elapsed?.Invoke();
        }
    }
}