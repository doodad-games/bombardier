using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BombButton : MonoBehaviour, IPointerDownHandler
{
    public static event Action onBomb;

    public void OnPointerDown(PointerEventData eventData) =>
        TryPlaceBomb();

    void Update()
    {
        if (Input.GetButtonDown("Jump")) { TryPlaceBomb(); }
    }

    void TryPlaceBomb()
    {
        if (Player.GameOver != null || GameplayUI.MenuShowing) { return; }

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

        onBomb?.Invoke();
        Sounds.PlaceBomb.Play();
        ++Stats.BombsPlaced;
    }
}