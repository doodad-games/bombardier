using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MusicButton : MonoBehaviour, IPointerClickHandler
{
    static MusicButton _instance;
    static bool _musicOff;
    static Action<bool> _musicChanged;

    static MusicButton() =>
        SoundButton.SubSound((on) =>
            _instance?._animator.SetBool("SoundOn", on)
        );

    public static bool MusicOn
    {
        get { return !_musicOff; }
        set
        {
            _musicOff = !value;
            PlayerPrefs.SetInt("MusicOff", value ? 0 : 1);
            
            _musicChanged?.Invoke(value);
            _instance._animator.SetBool("MusicOn", value);

            Sounds.BGMVolume(value);
        }
    }

    public static void SubMusic(Action<bool> cb) =>
        _musicChanged += cb;
    public static void UnsubMusic(Action<bool> cb) =>
        _musicChanged -= cb;

    Animator _animator;

    void Awake()
    {
        _instance = this;
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        MusicOn = PlayerPrefs.GetInt("MusicOff", 0) == 0;
        _animator.SetBool("SoundOn", SoundButton.SoundOn);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MusicOn = !MusicOn;
        Sounds.Click.Play();
    }
}