using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Kongregate : MonoBehaviour
{
#if UNITY_WEBGL
    [DllImport("__Internal")] static extern bool KongregateInit();
    [DllImport("__Internal")] static extern void KongregateSubmitStat(string stat, int value);
#else
    static bool KongregateInit() => false;
    static void KongregateSubmitStat(string stat, int value) {}
#endif

    static Kongregate _i;

    public static void SetStat(string name, int value)
    {
        if (!(_i?._connected ?? false)) return;
        KongregateSubmitStat(name, value);
    }

    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
#if !UNITY_WEBGL
        return;
#else
        var obj = new GameObject();
        obj.name = "Kongregate";
        DontDestroyOnLoad(obj);
        obj.AddComponent<Kongregate>();

        var exists = false;
        try { exists = KongregateInit(); }
        catch (EntryPointNotFoundException) { }

        if (!exists) Destroy(obj);
#endif
    }

    bool _connected;

    void OnEnable() => _i = this;

    void OnKongregateAPILoaded() => _connected = true;
}
