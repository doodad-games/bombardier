using System.Collections;
using UnityEngine;

public abstract class LootableObject : CustomTile
{
    #pragma warning disable CS0649
    [SerializeField]
    Lootable _type;
    #pragma warning restore CS0649

    protected abstract void OnLooted();

    bool _consumed;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (_consumed) { return; }

        var player = other.GetComponent<Player>();
        if (player == null) { return; }
        _consumed = true;

        OnLooted();

        StartCoroutine(Loot());
    }

    protected IEnumerator Loot()
    {
        var anim = GetComponent<Animator>();
        anim.SetFloat("LootedPlaybackSpeed", 1 / S.ObjectLootedShrinkTime);
        anim.SetTrigger("Looted");

        Instantiate(
            S.ObjectLootedParticle,
            transform.position,
            Quaternion.identity
        );

        CleanUp();

        ++Stats.LootCollected;
        Stats.IncLootableCount(_type);

        yield return new WaitForSeconds(S.ObjectLootedShrinkTime);

        Destroy(gameObject);
    }

    public override void Combust()
    {
        ++Stats.LootDestroyed;
        base.Combust();
    }
}