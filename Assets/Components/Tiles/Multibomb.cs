using UnityEngine;

public class Multibomb : LootableObject, IDropableTile
{
    public bool RollForDrop(Vector2Int pos) =>
        UnityEngine.Random.value <
            S.DropMultibombChance.Evaluate(-pos.y);

    protected override void OnLooted()
    {
        ++Player.Multibomb;
        Sounds.Multibomb.Play();
    }
}