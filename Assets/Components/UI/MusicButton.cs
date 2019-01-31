using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MusicButton : MonoBehaviour, IPointerClickHandler
{
    static MusicButton _instance;
    static bool _musicOff;

    static MusicButton() =>
        SoundButton.onSoundChanged += (on) =>
            _instance?._animator.SetBool("SoundOn", on);

    public static bool MusicOn
    {
        get { return !_musicOff; }
        set
        {
            _musicOff = !value;
            PlayerPrefs.SetInt("MusicOff", value ? 0 : 1);
            
            _instance._animator.SetBool("MusicOn", value);

            Sounds.BGMVolume(value);
        }
    }

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
        if (!SoundButton.SoundOn) return;

        MusicOn = !MusicOn;
        Sounds.Click.Play();
    }
}