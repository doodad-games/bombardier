using UnityEngine;

public class MainCamera : MonoBehaviour
{
    static MainCamera _instance;

    static MainCamera() =>
        Player.SubGameOver(
            (c) =>
                _instance
                    ._animator
                    .SetTrigger("ZoomIn")
        );
    
    Animator _animator;

    void Awake()
    {
        _instance = this;
        
        _animator = GetComponent<Animator>();
    }

    void Start() =>
        _animator.SetFloat("PlaybackSpeed", 1 / S.MainCameraZoomTime);
}