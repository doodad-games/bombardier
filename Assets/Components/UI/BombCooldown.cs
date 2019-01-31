using System.Collections;
using UnityEngine;

public class BombCooldown : MonoBehaviour
{
    public static bool BombAvailable { get; private set; }

    static BombCooldown _instance;

    static BombCooldown()
    {
        BombButton.onBomb += () => _instance.BombPlaced();

        Player.onMultibombChanged += (m) =>
            _instance._Cooldown = 
                S.AvailableBombInitialCooldown -
                m * S.PowerupMultibombDifference;
    }

    int _BombsAvailable
    {
        get { return _bombsAvailable; }
        set
        {
            _bombsAvailable = value;
            BombAvailable = _bombsAvailable != 0;
        }
    }

    float _cooldown;
    float _Cooldown
    {
        get { return _cooldown; }
        set
        {
            _cooldown = value;
            _onCooldown?.SetFloat(
                "CooldownPlaybackSpeed", 1 / _cooldown
            );
        }
    }

    Animator _available;
    Animator _onCooldown;
    int _bombsAvailable;

    float _cooldownElapsed;

    void Awake() =>
        _instance = this;

    void Start()
    {
        _BombsAvailable = S.AvailableBombLimit;
        _cooldown = S.AvailableBombInitialCooldown;

        for (var i = 0; i != _bombsAvailable; ++i)
        {
            var anim = CreateBomb();

            if (i == 0) { _available = anim; }
        }
    }

    void Update()
    {
        if (_onCooldown == null) { return; }

        _cooldownElapsed += Time.deltaTime;

        while (_cooldownElapsed > _cooldown)
        {
            var nextI = _onCooldown.transform.GetSiblingIndex() + 1;
            _onCooldown = nextI == transform.childCount
                ? null
                : transform.GetChild(nextI).GetComponent<Animator>();

            _onCooldown?.SetFloat("CooldownPlaybackSpeed", 1 / _cooldown);
            
            ++_BombsAvailable;
            _cooldownElapsed -= _cooldown;
        }
    }

    void BombPlaced()
    {
        --_BombsAvailable;

        var next = transform.GetChild(
            _available.transform.GetSiblingIndex() + 1
        ).GetComponent<Animator>();

        StartCoroutine(RemoveAvailableBomb());

        _available = next;

        var newCooldown = CreateBomb();

        newCooldown.SetTrigger("OnCooldown");

        if (_onCooldown == null)
        {
            _cooldownElapsed = 0f;
            _onCooldown = newCooldown;
            _onCooldown.SetFloat("CooldownPlaybackSpeed", 1 / _cooldown);
        }
    }

    Animator CreateBomb()
    {
        var anim = Instantiate(
            S.AvailableBombPrefab,
            transform
        ).GetComponent<Animator>();

        anim.SetFloat(
            "GrowShrinkPlaybackSpeed",
            1 / S.AvailableBombGrowShrinkTime
        );

        return anim;
    }

    IEnumerator RemoveAvailableBomb()
    {
        var bomb = _available;

        bomb.SetTrigger("Shrink");

        yield return new WaitForSeconds(S.AvailableBombGrowShrinkTime);

        Destroy(bomb.gameObject);
    }
}