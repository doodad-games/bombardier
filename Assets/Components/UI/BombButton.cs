using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BombButton : MonoBehaviour, IPointerDownHandler
{
    static Action _placed;

    public static void SubPlaced(Action cb) =>
        _placed += cb;
    public static void UnsubPlaced(Action cb) =>
        _placed -= cb;

    public void OnPointerDown(PointerEventData eventData) =>
        TryPlaceBomb();

    void Update()
    {
        if (Input.GetButtonDown("Jump")) { TryPlaceBomb(); }
    }

    void TryPlaceBomb()
    {
        if (Player.GameOver) { return; }

        if (!BombCooldown.BombAvailable)
        {
            Sounds.NoBomb.Play();
            return;
        }
        
        var closest = Player.ClosestEmptyTile;
        if (closest == null) { return; }

        var bomb = TileController.SpawnTile(S.TilePlayerBomb, closest.Value)
            as PlayerBomb;
        bomb.Initialise(Player.BombPower);

        _placed?.Invoke();

        Sounds.PlaceBomb.Play();
    }
}