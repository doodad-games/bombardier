using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour, IPointerClickHandler
{
    public static event Action onPressed;

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
        if (!_active || _restarting) { return; }
        _restarting = true;

        Sounds.Click.Play();
        onPressed?.Invoke();
    }
}
