using UnityEngine;
using UnityEngine.Audio;

public class Sounds : MonoBehaviour
{
    public static AudioSource Click =>
        _instance._click;
    public static AudioSource Gold =>
        _instance._gold;
    public static AudioSource Emerald =>
        _instance._emerald;
    public static AudioSource Diamond =>
        _instance._diamond;
    public static AudioSource Movement =>
        _instance._movement;
    public static AudioSource ExtraBombPower =>
        _instance._extraBombPower;
    public static AudioSource ExtraLife =>
        _instance._extraLife;
    public static AudioSource ExtraSpeed =>
        _instance._extraSpeed;
    public static AudioSource ExtraTime =>
        _instance._extraTime;
    public static AudioSource Multibomb =>
        _instance._multibomb;
    public static AudioSource LoseLife =>
        _instance._loseLife;
    public static AudioSource MineTrigger =>
        _instance._mineTrigger;
    public static AudioSource NewGame =>
        _instance._newGame;
    public static AudioSource NoBomb =>
        _instance._noBomb;
    public static AudioSource PlaceBomb =>
        _instance._placeBomb;
    public static AudioSource PlayerBurn =>
        _instance._playerBurn;
    public static AudioSource TimeOut =>
        _instance._timeOut;
    
    public static void MasterVolume(bool on) =>
        _instance._audioMixer.SetFloat(
            "master-volume",
            on ? S.AudioMasterVolumeOn : S.AudioVolumeOff
        );
    public static void BGMVolume(bool on) =>
        _instance._audioMixer.SetFloat(
            "bgm-volume",
            on ? 0f : S.AudioVolumeOff
        );

    public static void Burn()
    {
        var burns = _instance._burns;
        burns[UnityEngine.Random.Range(0, burns.Length)].Play();
    }

    public static void Explode()
    {
        var explodes = _instance._explodes;
        explodes[UnityEngine.Random.Range(0, explodes.Length)].Play();
    }

    static Sounds _instance;

    #pragma warning disable CS0649
    [SerializeField]
    AudioMixer _audioMixer;
    [SerializeField]
    AudioSource[] _burns;
    [SerializeField]
    AudioSource _click;
    [SerializeField]
    AudioSource _gold;
    [SerializeField]
    AudioSource _emerald;
    [SerializeField]
    AudioSource _diamond;
    [SerializeField]
    AudioSource _movement;
    [SerializeField]
    AudioSource[] _explodes;
    [SerializeField]
    AudioSource _extraBombPower;
    [SerializeField]
    AudioSource _extraLife;
    [SerializeField]
    AudioSource _extraSpeed;
    [SerializeField]
    AudioSource _extraTime;
    [SerializeField]
    AudioSource _multibomb;
    [SerializeField]
    AudioSource _loseLife;
    [SerializeField]
    AudioSource _mineTrigger;
    [SerializeField]
    AudioSource _newGame;
    [SerializeField]
    AudioSource _noBomb;
    [SerializeField]
    AudioSource _placeBomb;
    [SerializeField]
    AudioSource _playerBurn;
    [SerializeField]
    AudioSource _timeOut;
    #pragma warning restore CS0649

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
