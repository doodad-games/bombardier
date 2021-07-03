using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AchievementsLootable : MonoBehaviour
{
    public static event Action<Sprite, string, Lootable> onLootablePressed;

#pragma warning disable CS0649
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _count;
    [SerializeField] Lootable _type;
#pragma warning restore CS0649

    Sprite _ogSprite;
    string _ogName;
    bool _pressable;

    public void Clicked()
    {
        if (_pressable)
        {
            Sounds.Click.Play();
            onLootablePressed?.Invoke(_ogSprite, _ogName, _type);
        }
        else
        {
            Sounds.Error.Play();
        }
    }

    void Awake()
    {
        _ogSprite = _image.sprite;
        _ogName = _name.text;
    }

    void OnEnable()
    {
        Stats.onStatsRefreshed += Redraw;
        Redraw();
    }

    void OnDisable() =>
        Stats.onStatsRefreshed -= Redraw;

    void Redraw()
    {
        var count = Stats.GetLootableCount(_type);

        if (count == 0)
        {
            _pressable = false;
            _image.sprite = S.SpriteLootableUnknown;
            _name.text = "Unknown";
            _count.text = "0";
        }
        else
        {
            _pressable = true;
            _image.sprite = _ogSprite;
            _name.text = _ogName;

            var diff = Stats.GetLootableCountDiff(_type);

            _count.text = diff == 0
                ? count.ToString()
                : string.Format(
                    "{0} <b><color=green>+ {1}</color></b>",
                    count - diff,
                    diff
                );
        }
    }
}
