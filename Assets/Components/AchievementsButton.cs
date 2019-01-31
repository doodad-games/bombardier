using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AchievementsButton : MonoBehaviour, IPointerClickHandler
{
    public static event Action onPressed;

    public void OnPointerClick(PointerEventData eventData)
    {
        Sounds.Click.Play();
        onPressed?.Invoke();
    }
}
