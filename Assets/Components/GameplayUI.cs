using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    public static event Action onMenuHidden;

    public static bool MenuShowing { get; private set; }

    static GameplayUI _instance;

    static GameplayUI()
    {
        MenuShowing = true;

        Player.onExtraLifeChanged += (active) =>
            _instance._animator.SetTrigger(
                "ExtraLife" + (active ? "Gain" : "Consume")
            );

        Player.onGameOver += (c) =>
            _instance.StartCoroutine(_instance.ShowScore());

        PlayButton.onPressed += () =>
        {
            var wasShowing = MenuShowing;
            MenuShowing = false;

            Sounds.NewGame.Play();

            if (wasShowing)
            {
                onMenuHidden?.Invoke();

                _instance._animator.SetTrigger("StartGame");
                ++Stats.GamesStarted;
            }
            else _instance.StartCoroutine(_instance.EndGame());
        };

        MenuButton.onPressed += () =>
        {
            MenuShowing = true;
            _instance.StartCoroutine(_instance.EndGame());
        };

        AchievementsButton.onPressed += () =>
            _instance._achievements.SetActive(true);
        
        AchievementsLootable.onLootablePressed += (sprite, name, type) =>
            _instance._lootablePopup.Activate(sprite, name, type);
    }

    #pragma warning disable CS0649
    [SerializeField]
    GameObject _menu;
    [SerializeField]
    GameObject _achievements;
    [SerializeField]
    LootablePopup _lootablePopup;
    #pragma warning restore CS0649
        
    Animator _animator;

    void Awake()
    {
        _instance = this;

        _animator = GetComponent<Animator>();

        if (!MenuShowing)
        {
            _menu.SetActive(false);
            _animator.SetTrigger("StartGame");
            ++Stats.GamesStarted;
        }
    }

    void Start() =>
        _animator.SetFloat("MainPlaybackSpeed", 1 / S.GameplayUIAnimTime);

    IEnumerator ShowScore()
    {
        yield return new WaitForSecondsRealtime(S.MainCameraZoomTime);

        _achievements.SetActive(true);
        _animator.SetTrigger("ShowScore");
    }
    
    IEnumerator EndGame()
    {
        _animator.SetTrigger("FadeOut");

        yield return new WaitForSecondsRealtime(S.GameplayUIAnimTime);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}
