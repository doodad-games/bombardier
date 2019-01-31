using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Achievements : MonoBehaviour, IPointerClickHandler
{
    #pragma warning disable CS0649
    [SerializeField]
    GameObject _closeButton;
    [SerializeField]
    RectTransform _scrollContents;
    [SerializeField]
    TextMeshProUGUI _totalScore;
    [SerializeField]
    TextMeshProUGUI _highestScore;
    [SerializeField]
    TextMeshProUGUI _distanceTravelled;
    [SerializeField]
    TextMeshProUGUI _depthTravelled;
    [SerializeField]
    TextMeshProUGUI _deepestReached;
    [SerializeField]
    TextMeshProUGUI _timePlayed;
    [SerializeField]
    TextMeshProUGUI _gamesStarted;
    [SerializeField]
    TextMeshProUGUI _lossesByTime;
    [SerializeField]
    TextMeshProUGUI _lossesByOwnBomb;
    [SerializeField]
    TextMeshProUGUI _extraLivesLost;
    [SerializeField]
    TextMeshProUGUI _bombsPlaced;
    [SerializeField]
    TextMeshProUGUI _rocksDestroyed;
    [SerializeField]
    TextMeshProUGUI _lootDestroyed;
    [SerializeField]
    TextMeshProUGUI _lootCollected;
    #pragma warning restore CS0649

    Animator _animator;

    public void Close()
    {
        Sounds.Click.Play();
        _animator.SetTrigger("Close");
    }

    public void Disable() => gameObject.SetActive(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_closeButton.activeSelf) Close();
    }

    void Awake() =>
        _animator = GetComponent<Animator>();

    void OnEnable()
    {
        var fromPlay = !GameplayUI.MenuShowing;

        _scrollContents.anchoredPosition = Vector2.zero;

        _closeButton.SetActive(!fromPlay);

        _totalScore.text = StatWithDiff(Stats.TotalScore, Score.Amount);

        _highestScore.text = Score.IsHighscore
            ? string.Format("<b><color=green>{0}</color></b>", Score.Amount)
            : Stats.HighestScore.ToString();

        _distanceTravelled.text = StatWithDiff(
            Stats.DistanceTravelled,
            Stats.DistanceTravelledDiff
        ) + " metres";

        _depthTravelled.text = StatWithDiff(
            Stats.DepthTravelled,
            Stats.DepthTravelledDiff
        ) + " metres";

        _deepestReached.text = (
            Player.IsDeepest
                ? string.Format("<b><color=green>{0}</color></b>", Stats.DeepestReached)
                : Stats.DeepestReached.ToString()
        ) + " metres";

        _timePlayed.text = StatWithDiff(
            (int)Stats.TimePlayed,
            (int)Timer.TimePassed
        ) + " seconds";

        _gamesStarted.text = StatWithDiff(Stats.GamesStarted, fromPlay ? 1 : 0);

        _lossesByTime.text = StatWithDiff(
            Stats.LossesByTime,
            Player.GameOver == Player.CauseOfGameOver.Time ? 1 : 0,
            "orange"
        );

        _lossesByOwnBomb.text = StatWithDiff(
            Stats.LossesByOwnBomb,
            Player.GameOver == Player.CauseOfGameOver.PlayerBomb ? 1 : 0,
            "orange"
        );

        _extraLivesLost.text = StatWithDiff(
            Stats.ExtraLivesLost,
            Stats.ExtraLivesLostDiff,
            "orange"
        );

        _bombsPlaced.text = StatWithDiff(
            Stats.BombsPlaced,
            Stats.BombsPlacedDiff
        );

        _rocksDestroyed.text = StatWithDiff(
            Stats.RocksDestroyed,
            Stats.RocksDestroyedDiff
        );

        _lootDestroyed.text = StatWithDiff(
            Stats.LootDestroyed,
            Stats.LootDestroyedDiff,
            "orange"
        );

        _lootCollected.text = StatWithDiff(
            Stats.LootCollected,
            Stats.LootCollectedDiff
        );
    }

    string StatWithDiff(int original, int diff, string colour = "green") =>
        diff == 0
            ? original.ToString()
            : string.Format(
                "{1} <b><color={0}>+ {2}</color></b>",
                colour,
                original - diff,
                diff
            );
}
