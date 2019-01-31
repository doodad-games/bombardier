using UnityEngine;

public class MainCamera : MonoBehaviour
{
    static MainCamera _instance;

    static MainCamera()
    {
        Player.onGameOver += (c) =>
            _instance
                ._animator
                .SetTrigger("ZoomIn");

        GameplayUI.onMenuHidden += () =>
            _instance
                ._animator
                .SetTrigger("ZoomOut");
    }
    
    Animator _animator;

    void Awake()
    {
        _instance = this;
        
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _animator.SetFloat("PlaybackSpeed", 1 / S.MainCameraZoomTime);

        if (!GameplayUI.MenuShowing) _animator.SetTrigger("ZoomOut");
    }
}