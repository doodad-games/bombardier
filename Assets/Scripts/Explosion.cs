using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Explosion : MonoBehaviour
{
    [HideInInspector] public Source source;

    bool _burnCompleted;
    List<ICustomTile> _tilesHit;

    protected virtual void Start()
    {
        GetComponent<Animator>()
            .SetFloat("PlaybackSpeed", 1 / S.ExplosionLifetime);

        StartCoroutine(Lifecycle());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_burnCompleted) { return; }

        var player = other.GetComponent<Player>();
        if (player == null) { return; }

        player.Burn(source);
    }

    protected IEnumerator Combust(ICustomTile tile, bool preventDrops = false)
    {
        if (tile is PlayerBomb)
        {
            tile.Combust();
            yield break;
        }

        yield return new WaitForSeconds(S.ExplosionCombustAfter);
        
        if (tile is Rock)
        {
            (tile as Rock).preventDrops = preventDrops;
        }

        tile.Combust();
        if (!tile.GoneAfterCombust())
        {
            tile.IncFireCount();
            
            if (_tilesHit == null)
            {
                _tilesHit = new List<ICustomTile>();
            }
            _tilesHit.Add(tile);
        }
    }

    IEnumerator Lifecycle()
    {
        yield return new WaitForSeconds(
            S.ExplosionLifetime * S.ExplosionEndBurnAt
        );

        _burnCompleted = true;

        if (_tilesHit != null)
        {
            foreach (var tile in _tilesHit)
            {
                tile.DecFireCount();
            }
        }

        Destroy(
            gameObject,
            S.ExplosionLifetime * (1 - S.ExplosionEndBurnAt)
        );
    }

    [Serializable]
    public enum Source
    {
        PlayerBomb,
        Mine
    }
}