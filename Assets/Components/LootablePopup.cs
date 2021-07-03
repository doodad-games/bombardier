using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootablePopup : MonoBehaviour
{
    #pragma warning disable CS0649
    [SerializeField]
    GameObject _closeButton;
    [SerializeField]
    Image _image;
    [SerializeField]
    TextMeshProUGUI _name;
    [SerializeField]
    TextMeshProUGUI _count;
    [SerializeField]
    TextMeshProUGUI _description;
    #pragma warning restore CS0649

    Animator _animator;

    public void Activate(Sprite sprite, string name, Lootable type)
    {
        _image.sprite = sprite;
        _name.text = name;
        _count.text = string.Format("Collected: {0}", Stats.GetLootableCount(type));
        _description.text = S.LootableDescriptions[type];

        gameObject.SetActive(true);
    }

    void Awake() =>
        _animator = GetComponent<Animator>();
}
