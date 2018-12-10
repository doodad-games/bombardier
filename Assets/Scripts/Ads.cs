using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour
{
    public static bool Showing { get; private set; }

    static Ads _instance;
    static float _nextAdAfter = -1f;

    static Ads() =>
        Player.SubGameOver((c) =>
        {
            if (
                _instance == null ||
                Time.time < _nextAdAfter
            ) { return; }
            
            _instance.StartCoroutine(_instance.LaunchAd());
            _nextAdAfter = Time.time +
                (_instance._testMode ? 0f : S.AdMinTimeBetween);
        });
    
    #if UNITY_EDITOR
    #pragma warning disable CS0649
    [SerializeField]
    bool _testMode;
    #pragma warning restore CS0649
    #else
    bool _testMode = false;
    #endif
    
    void Awake()
    {
        if (!Advertisement.isSupported || _instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        DontDestroyOnLoad(gameObject);
    }
    
    void Start() =>
        _nextAdAfter = _testMode ? 0f : S.AdFirstAfter;

    IEnumerator LaunchAd()
    {
        Advertisement.IsReady();

        yield return new WaitForSecondsRealtime(S.AdDelay);

        var giveUpAfter = Time.unscaledTime + S.AdGiveUpAfter;
        
        while (!Advertisement.IsReady())
        {
            if (Time.unscaledTime > giveUpAfter)
            {
                yield break;
            }

            yield return new WaitForSecondsRealtime(S.AdCheckPeriod);
        }

        var ts = Time.timeScale;
        Time.timeScale = 0f;

        Showing = true;
        Advertisement.Show();

        while (Advertisement.isShowing)
        {
            yield return new WaitForSecondsRealtime(S.AdCheckPeriod);
        }

        Showing = false;
        Time.timeScale = ts;
    }
}