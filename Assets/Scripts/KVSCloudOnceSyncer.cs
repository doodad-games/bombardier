#if UNITY_IOS || CLOUDONCE_GOOGLE
using System;
using System.Collections.Generic;
using System.Linq;
using CloudOnce;
using CloudOnce.CloudPrefs;
using MyLibrary;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class KVSCloudOnceSyncer : MonoBehaviour
{
    public static event Action onSyncedFromCloudOnce;
    public static event Action onStatusChanged;

    const float CLOUDONCE_UPDATE_RETRY_PERIOD = 5f;
    const string KVS_CLOUDONCE_KEY = "kvs";
    const string KVS_CLOUD_SAVE_ON = "cloudSaveOn";
    const string CLOUDONCE_QUOTE_REPLACEMENT = "__kvs-quote__";

    static CloudString _cloudOnceStr = new CloudString(
        KVS_CLOUDONCE_KEY,
        PersistenceType.Latest,
        "{}"
    );

    public static KVSCloudOnceSyncer I { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        var obj = new GameObject("KVSCloudOnceSyncer");
        DontDestroyOnLoad(obj);
        obj.AddComponent<KVSCloudOnceSyncer>();
    }

    Status _status = Status.Disabled;
    bool _initdCloudOnce;
    bool _needsLoad;
    bool _quitting;
    float _tryUpdatingCloudOnceFrom = float.MaxValue;

    public Status CurStatus
    {
        get => _status;
        set
        {
            _status = value;
            onStatusChanged?.Invoke();

            Debug.Log("KVSCloudOnceSyncer status changed: " + _status.ToString());
        }
    }

    public bool Disabled => _status == Status.Disabled;

    void OnEnable()
    {
        if (I != null && I != this)
        {
            Debug.LogError("Duplicate KVSCloudOnceSyncer created, self-destructing");
            Destroy(gameObject);
            return;
        }
        I = this;

        Application.quitting += HandleApplicationQuitting;
        Cloud.OnCloudLoadComplete += HandleCloudLoadComplete;
        Cloud.OnCloudSaveComplete += HandleCloudSaveComplete;
        Cloud.OnNewCloudValues += HandleCloudOnceChanged;
        KVS.onSave += HandleKVSSaved;
    }

    void Start() =>
        TurnOnOff(KVS.GetInt(KVS_CLOUD_SAVE_ON, false ? 1 : 0) == 1);

    void Update() =>
        TrySyncingCloudOnceStorage(false);

    void OnDisable()
    {
        Application.quitting -= HandleApplicationQuitting;
        Cloud.OnCloudLoadComplete -= HandleCloudLoadComplete;
        Cloud.OnCloudSaveComplete -= HandleCloudSaveComplete;
        Cloud.OnNewCloudValues -= HandleCloudOnceChanged;
        KVS.onSave -= HandleKVSSaved;
    }

    public void TurnOnOff(bool on)
    {
        KVS.SetInt(KVS_CLOUD_SAVE_ON, on ? 1 : 0);

        if (on)
        {
            if (!_initdCloudOnce)
            {
                _initdCloudOnce = true;
                Debug.Log("KVSToCloudOnceSyncer: Initialising CloudOnce");
                Cloud.Initialize();

                _tryUpdatingCloudOnceFrom = Time.unscaledTime + CLOUDONCE_UPDATE_RETRY_PERIOD;
            }

            CurStatus = Status.Pending;
            
            _needsLoad = true;
            TrySyncingCloudOnceStorage(true);
        }
        else
        {
            CurStatus = Status.Disabled;
            _tryUpdatingCloudOnceFrom = float.MaxValue;
        }
    }

    void HandleApplicationQuitting() => _quitting = true;

    void HandleCloudLoadComplete(bool success)
    {
        if (Disabled || !_needsLoad)
            return;

        if (success)
        {
            Debug.Log("KVSToCloudOnceSyncer: CloudOnce loaded data successfully");
            _needsLoad = false;

            if (MergeCloudOnceToKVS())
                SyncKVSToCloudOnce();
        }
        else
        {
            Debug.Log("KVSToCloudOnceSyncer: CloudOnce failed to load data");
            CurStatus = Status.Failed;
        }
    }

    void HandleCloudSaveComplete(bool success)
    {
        if (Disabled || _needsLoad)
            return;

        if (success)
        {
            Debug.Log("KVSToCloudOnceSyncer: CloudOnce saved data successfully");
            _tryUpdatingCloudOnceFrom = float.MaxValue;
            CurStatus = Status.Synced;
        }
        else
        {
            Debug.Log("KVSToCloudOnceSyncer: CloudOnce failed to save data");
            CurStatus = Status.Failed;
        }
    }

    void HandleCloudOnceChanged(string[] changedKeys)
    {
        if (
            Disabled ||
            _quitting ||
            Array.IndexOf(changedKeys, KVS_CLOUDONCE_KEY) == -1
        ) return;

        CurStatus = Status.Pending;

        Debug.Log("KVSToCloudOnceSyncer: CloudOnce changed its stored KVS - merging into KVS and pushing back to CloudOnce");

        if (MergeCloudOnceToKVS())
            SyncKVSToCloudOnce();
    }

    void HandleKVSSaved()
    {
        if (Disabled || _needsLoad || _quitting)
            return;

        CurStatus = Status.Pending;

        Debug.Log("KVSToCloudOnceSyncer: KVS saved some data locally - pushing to CloudOnce");

        SyncKVSToCloudOnce();
    }

    bool MergeCloudOnceToKVS()
    {
        KVS.Data cloudOnceData;
        try
        {
            cloudOnceData = JsonUtility.FromJson<KVS.Data>(
                _cloudOnceStr.Value.Replace(CLOUDONCE_QUOTE_REPLACEMENT, "\"")
            );
        }
        catch (Exception)
        {
            Debug.LogWarning("KVSToCloudOnceSyncer: Invalid KVS data in CloudOnce, ignoring. Data:\n\n" + _cloudOnceStr.Value);
            return false;
        }

        KVS.RawData = DoMerge(cloudOnceData, KVS.RawData);
        KVS.Save();
        onSyncedFromCloudOnce?.Invoke();

        return true;
    }

    KVS.Data DoMerge(KVS.Data remote, KVS.Data local)
    {
        { // Floats
            var kvIndexes = new Dictionary<string, int>();
            for (var i = local.floats.Count - 1; i != -1; --i)
                kvIndexes[local.floats[i].key] = i;
            
            foreach (var remoteKV in remote.floats)
            {
                if (kvIndexes.ContainsKey(remoteKV.key))
                {
                    var i = kvIndexes[remoteKV.key];
                    if (remoteKV.val > local.floats[i].val)
                        local.floats[i] = remoteKV;
                }
                else
                    local.floats.Add(remoteKV);
            }
        }

        { // Ints
            var kvIndexes = new Dictionary<string, int>();
            for (var i = local.ints.Count - 1; i != -1; --i)
                kvIndexes[local.ints[i].key] = i;
            
            foreach (var remoteKV in remote.ints)
            {
                if (kvIndexes.ContainsKey(remoteKV.key))
                {
                    var i = kvIndexes[remoteKV.key];
                    if (remoteKV.val > local.ints[i].val)
                        local.ints[i] = remoteKV;
                }
                else
                    local.ints.Add(remoteKV);
            }
        }

        return local;
    }

    void SyncKVSToCloudOnce()
    {
        _cloudOnceStr.Value = JsonUtility.ToJson(KVS.RawData)
            .Replace("\"", CLOUDONCE_QUOTE_REPLACEMENT);

        TrySyncingCloudOnceStorage(true);
    }

    void TrySyncingCloudOnceStorage(bool force)
    {
        if (
            Disabled ||
            (!force && Time.unscaledTime < _tryUpdatingCloudOnceFrom)
        ) return;

        _tryUpdatingCloudOnceFrom = Time.unscaledTime + CLOUDONCE_UPDATE_RETRY_PERIOD;

        if (_needsLoad)
        {
            Debug.Log("KVSToCloudOnceSyncer: Trying to load CloudOnce data");
            Cloud.Storage.Load();
        }
        else
        {
            Debug.Log("KVSToCloudOnceSyncer: Trying to save CloudOnce data");
            _cloudOnceStr.Flush();
            Cloud.Storage.Save();
        }
    }

    public enum Status
    {
        Disabled,
        Pending,
        Failed,
        Synced
    }
}
#endif