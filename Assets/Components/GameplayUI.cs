using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    static GameplayUI _instance;

    static GameplayUI()
    {
        Player.SubExtraLife((active) =>
            _instance._animator.SetTrigger(
                "ExtraLife" + (active ? "Gain" : "Consume")
            )
        );

        Player.SubGameOver((c) =>
            _instance._animator.SetTrigger("ShowScore")
        );

        RestartButton.SubPressed(() =>
        {
            _instance.StartCoroutine(_instance.EndGame());
        });
    }
        
    Animator _animator;

    void Awake()
    {
        _instance = this;

        _animator = GetComponent<Animator>();
    }

    void Start() =>
        _animator.SetFloat("MainPlaybackSpeed", 1 / S.GameplayUIAnimTime);
    
    IEnumerator EndGame()
    {
        _animator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(S.GameplayUIAnimTime);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}
