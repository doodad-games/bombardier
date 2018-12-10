using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    bool _initalised;
    bool _isMine;
    bool _burnCompleted;
    List<ICustomTile> _tilesHit;

    protected void Initialise(bool isMine)
    {
        if (_initalised) { throw new NotSupportedException(); }
        _initalised = true;

        _isMine = isMine;
    }

    protected virtual void Start()
    {
        if (!_initalised) { throw new NotSupportedException(); }

        GetComponent<Animator>()
            .SetFloat("PlaybackSpeed", 1 / S.ExplosionLifetime);

        StartCoroutine(Lifecycle());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_burnCompleted) { return; }

        var player = other.GetComponent<Player>();
        if (player == null) { return; }

        player.Burn(_isMine);
    }

    protected IEnumerator Combust(ICustomTile tile, bool preventDrops = false)
    {
        if (tile is PlayerBomb || tile is Mine)
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
}