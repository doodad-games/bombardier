using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int Amount { get; private set; }
    public static bool IsHighscore { get; private set; }

    static Score _instance;

    public static void Add(int amount)
    {
        Amount += amount;

        Stats.TotalScore += amount;
        if (Amount > Stats.HighestScore)
        {
            Stats.HighestScore = Amount;
            IsHighscore = true;
        }

        _instance._text.text = Amount.ToString();
    }

    #pragma warning disable CS0649
    [SerializeField]
    TextMeshProUGUI _text;
    #pragma warning restore CS0649

    void Awake()
    {
        _instance = this;
        Amount = 0;
        IsHighscore = false;
    }
}