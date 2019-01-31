using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundButton : MonoBehaviour, IPointerClickHandler
{
    public static event Action<bool> onSoundChanged;

    static SoundButton _instance;
    static bool _soundOff;

    public static bool SoundOn
    {
        get { return !_soundOff; }
        set
        {
            _soundOff = !value;
            PlayerPrefs.SetInt("SoundOff", value ? 0 : 1);
            
            onSoundChanged?.Invoke(value);
            _instance._animator.SetBool("SoundOn", value);

            Sounds.MasterVolume(value);
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
        SoundOn = PlayerPrefs.GetInt("SoundOff", 0) == 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundOn = !SoundOn;
        Sounds.Click.Play();
    }
}