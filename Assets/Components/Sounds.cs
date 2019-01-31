using UnityEngine;
using UnityEngine.Audio;

public class Sounds : MonoBehaviour
{
    static Sounds _instance;

    #pragma warning disable CS0649
    public static AudioSource Click =>
        _instance._click;
    [SerializeField] AudioSource _click;
    public static AudioSource Points1 =>
        _instance._points1;
    [SerializeField] AudioSource _points1;
    public static AudioSource Points2 =>
        _instance._points2;
    [SerializeField] AudioSource _points2;
    public static AudioSource Points3 =>
        _instance._points3;
    [SerializeField] AudioSource _points3;
    public static AudioSource Movement =>
        _instance._movement;
    [SerializeField] AudioSource _movement;
    public static AudioSource Error =>
        _instance._error;
    [SerializeField] AudioSource _error;
    public static AudioSource ExtraBombPower =>
        _instance._extraBombPower;
    [SerializeField] AudioSource _extraBombPower;
    public static AudioSource ExtraLife =>
        _instance._extraLife;
    [SerializeField] AudioSource _extraLife;
    public static AudioSource ExtraSpeed =>
        _instance._extraSpeed;
    [SerializeField] AudioSource _extraSpeed;
    public static AudioSource ExtraTime =>
        _instance._extraTime;
    [SerializeField] AudioSource _extraTime;
    public static AudioSource Multibomb =>
        _instance._multibomb;
    [SerializeField] AudioSource _multibomb;
    public static AudioSource LoseLife =>
        _instance._loseLife;
    [SerializeField] AudioSource _loseLife;
    public static AudioSource NewGame =>
        _instance._newGame;
    [SerializeField] AudioSource _newGame;
    public static AudioSource NoBomb =>
        _instance._noBomb;
    [SerializeField] AudioSource _noBomb;
    public static AudioSource PlaceBomb =>
        _instance._placeBomb;
    [SerializeField] AudioSource _placeBomb;
    public static AudioSource PlayerBurn =>
        _instance._playerBurn;
    [SerializeField] AudioSource _playerBurn;
    public static AudioSource TimeOut =>
        _instance._timeOut;
    [SerializeField] AudioSource _timeOut;
    public static AudioSource HighScore =>
        _instance._highScore;
    [SerializeField] AudioSource _highScore;
    #pragma warning restore CS0649
    
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

    #pragma warning disable CS0649
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] AudioSource[] _burns;
    [SerializeField] AudioSource[] _explodes;
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
