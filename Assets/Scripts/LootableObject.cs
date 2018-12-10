using System.Collections;
using UnityEngine;

public abstract class LootableObject : CustomTile
{
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

        yield return new WaitForSeconds(S.ObjectLootedShrinkTime);

        Destroy(gameObject);
    }
}