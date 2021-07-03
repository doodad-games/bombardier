using System;
using UnityEngine;

public abstract class CustomTile : MonoBehaviour, ICustomTile
{
    GameObject ICustomTile.GameObject() => gameObject;
    StaticTileData ICustomTile.Data() => _tileData;
    void ICustomTile.IncFireCount() => ++_FireCount;
    void ICustomTile.DecFireCount() => --_FireCount;

    #pragma warning disable CS0649
    [SerializeField]
    SpriteRenderer _renderer;
    [SerializeField]
    StaticTileData _tileData;
    #pragma warning restore CS0649

    public virtual bool Unwalkable() => false;
    public virtual bool NeedsPierce() => false;
    public virtual bool GoneAfterCombust() => true;
    public virtual bool CapsExplosions() => false;

    protected bool Initialised { get; private set; }
    protected bool IsDropped { get; private set; }
    protected bool Cleaned { get; private set; }

    Action _onDestroy;

    int _fireCount;
    bool _destroyed;

    int _FireCount
    {
        get { return _fireCount; }
        set
        {
            _fireCount = value;
            if (value == 0) { OnBurn(); }
        }
    }
    
    public void SubOnDestroy(Action cb) =>
        _onDestroy += cb;
    public void UnsubOnDestroy(Action cb) =>
        _onDestroy -= cb;

    public virtual void Initialise(bool isDropped)
    {
        if (Initialised) { throw new NotSupportedException(); }
        Initialised = true;

        this.IsDropped = isDropped;
    }

    public virtual void Combust()
    {
        if (Cleaned) { return; }

        if (GoneAfterCombust())
        {
            Sounds.Burn();
            CleanUp();
            Destroy(gameObject);
        }
    }

    protected virtual void OnBurn() { }
    
    protected void CleanUp()
    {
        if (Cleaned) { return; }
        Cleaned = true;

        _onDestroy?.Invoke();
    }

    protected virtual void Awake()
    {
        if (_tileData?.Sprites != null)
        {
            _renderer.sprite = _tileData.Sprites[
                UnityEngine.Random.Range(0, _tileData.Sprites.Count)
            ];
        }
    }

    protected virtual void Start()
    {
        if (!Initialised) { throw new NotSupportedException(); }
    }

    void OnDestroy() =>
        CleanUp();
}
