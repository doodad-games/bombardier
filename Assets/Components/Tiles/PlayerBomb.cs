using System;
using System.Collections;
using UnityEngine;

public class PlayerBomb : CustomTile
{
    #pragma warning disable CS0649
    [SerializeField]
    Animator _bombFlashAnimator;
    #pragma warning restore CS0649

    bool _initialised;
    int _power;
    bool _combusting;

    public override bool GoneAfterCombust() => true;

    public void Initialise(int power)
    {
        if (_initialised) { throw new NotSupportedException(); }
        _initialised = true;

        _power = power;
    }

    public override void Combust()
    {
        if (_combusting) { return; }
        StartCoroutine(_Combust());
    }

    protected override void Start()
    {
        if (!_initialised) { throw new NotSupportedException(); }

        base.Start();

        _bombFlashAnimator.SetFloat(
            "PlaybackSpeed",
            1 / (S.PlayerBombExplodeAfter + S.PlayerBombDestroyAfter)
        );
        _bombFlashAnimator.SetTrigger("Burn");

        StartCoroutine(BurnFuse());
    }

    IEnumerator BurnFuse()
    {
        yield return new WaitForSeconds(
            S.PlayerBombExplodeAfter * S.PlayerBombCombustAt
        );

        if (_combusting) { yield break; }
        StartCoroutine(_Combust());
    }

    IEnumerator _Combust()
    {
        if (_combusting) { yield break; }
        _combusting = true;

        yield return new WaitForSeconds(
            S.PlayerBombExplodeAfter * (1 - S.PlayerBombCombustAt)
        );

        _bombFlashAnimator.Play(
            "BombFlash",
            0,
            S.PlayerBombCombustAt
        );

        Instantiate(
            S.PlayerExplosion,
            transform.position,
            Quaternion.identity
        )
            .GetComponent<PlayerExplosion>()
            .Initialise(_power);
        Sounds.Explode();

        Destroy(gameObject, S.PlayerBombDestroyAfter);
    }
}