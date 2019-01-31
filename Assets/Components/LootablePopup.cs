using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootablePopup : MonoBehaviour, IPointerClickHandler
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

    public void Close()
    {
        Sounds.Click.Play();
        _animator.SetTrigger("Close");
    }

    public void Disable() => gameObject.SetActive(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_closeButton.activeSelf) Close();
    }

    void Awake() =>
        _animator = GetComponent<Animator>();
}
