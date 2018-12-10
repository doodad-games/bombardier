using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RestartButton : MonoBehaviour, IPointerClickHandler
{
    static Action _pressed;

    public static void SubPressed(Action cb) =>
        _pressed += cb;
    public static void UnsubPressed(Action cb) =>
        _pressed -= cb;
    
    #pragma warning disable CS0649
    [SerializeField]
    bool _active;
    #pragma warning restore CS0649

    bool _restarting;

    public void OnPointerClick(PointerEventData eventData) =>
        Pressed();

    void Update()
    {
        if (Input.GetButtonDown("Jump")) { Pressed(); }
    }

    void Pressed()
    {
        if (!_active || _restarting || Ads.Showing) { return; }
        _restarting = true;

        Sounds.Click.Play();
        Sounds.NewGame.Play();
        _pressed?.Invoke();
    }
}
