using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Stats : MonoBehaviour
{
    static Stats() =>
        Player.SubGameOver((cause) =>
            Analytics.CustomEvent(
                "gameEnd",
                new Dictionary<string, object>()
                {   { "duration", Time.timeSinceLevelLoad }
                ,   { "score", Score.Amount }
                ,   { "deathVia", cause.ToString() }
                }
            )
        );
    
    void Awake() =>
        Analytics.CustomEvent("gameStart");
}
