using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static float Horizontal { get; private set; }
    public static float Vertical { get; private set; }

    #pragma warning disable CS0649
    [SerializeField]
    Collider2D _pad;
    [SerializeField]
    GameObject _thumb;
    #pragma warning restore CS0649

    bool _inUse;

    public void OnPointerDown(PointerEventData eventData)
    {
        _inUse = true;
        UpdateThumbPos(eventData);
    }

    public void OnDrag(PointerEventData eventData) =>
        UpdateThumbPos(eventData);

    public void OnPointerUp(PointerEventData eventData)
    {
        _inUse = false;
        _thumb.transform.position = _pad.bounds.center;

        Horizontal = 0f;
        Vertical = 0f;
    }
        
    void Update()
    {
        if (!_inUse) return;

        var thumbPos = _thumb.transform.position;

        Horizontal = 
            Mathf.Clamp(
                2f * (thumbPos.x - _pad.bounds.center.x) / _pad.bounds.size.x,
                -1f, 1f
            );
        Vertical =
            Mathf.Clamp(
                2f * (thumbPos.y - _pad.bounds.center.y) / _pad.bounds.size.y,
                -1f, 1f
            );
    }

    void OnDisable()
    {
        Horizontal = 0f;
        Vertical = 0f;
    }

    void UpdateThumbPos(PointerEventData eventData)
    {
        var pos = eventData.position;
        var newPos = _pad.bounds
            .ClosestPoint(new Vector3(pos.x, pos.y, 0f));

        _thumb.transform.position = newPos;
    }
}
