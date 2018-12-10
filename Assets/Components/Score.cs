using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int Amount
    {
        get { return _amount; }
        set
        {
            _amount = value;
            _instance._text.text = _amount.ToString();
        }
    }

    static Score _instance;
    static int _amount;

    #pragma warning disable CS0649
    [SerializeField]
    Text _text;
    #pragma warning restore CS0649

    void Awake()
    {
        _instance = this;
        Amount = 0;
    }
}