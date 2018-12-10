using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundButton : MonoBehaviour, IPointerClickHandler
{
    static SoundButton _instance;
    static bool _soundOff;
    static Action<bool> _soundChanged;

    public static bool SoundOn
    {
        get { return !_soundOff; }
        set
        {
            _soundOff = !value;
            PlayerPrefs.SetInt("SoundOff", value ? 0 : 1);
            
            _soundChanged?.Invoke(value);
            _instance._animator.SetBool("SoundOn", value);

            Sounds.MasterVolume(value);
        }
    }

    public static void SubSound(Action<bool> cb) =>
        _soundChanged += cb;
    public static void UnsubSound(Action<bool> cb) =>
        _soundChanged -= cb;

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