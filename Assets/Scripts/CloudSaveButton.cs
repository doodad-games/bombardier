using TMPro;
using UnityEngine;

public class CloudSaveButton : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] TextMeshProUGUI _statusText;
#pragma warning restore CS0649

#if UNITY_IOS || CLOUDONCE_GOOGLE
    void OnEnable()
    {
        KVSCloudOnceSyncer.onStatusChanged += Redraw;
        Redraw();
    }

    void OnDisable() =>
        KVSCloudOnceSyncer.onStatusChanged -= Redraw;
    
    public void Insp_Toggle()
    {
        KVSCloudOnceSyncer.I.TurnOnOff(KVSCloudOnceSyncer.I.Disabled);
        Sounds.Click.Play();
    }

    void Redraw()
    {
        switch (KVSCloudOnceSyncer.I.CurStatus)
        {
            case KVSCloudOnceSyncer.Status.Disabled:
                _statusText.text = "Disabled";
                break;
            case KVSCloudOnceSyncer.Status.Pending:
                _statusText.text = "Syncing...";
                break;
            case KVSCloudOnceSyncer.Status.Failed:
                _statusText.text = "Connecting...";
                break;
            case KVSCloudOnceSyncer.Status.Synced:
                _statusText.text = "Synced!";
                break;
        };
    }
#else
    void Awake() => gameObject.SetActive(false);

    public void Insp_Toggle() { }
#endif
}