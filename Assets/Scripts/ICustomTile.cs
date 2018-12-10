using System;
using UnityEngine;

public interface ICustomTile
{
    GameObject GameObject();

    bool Unwalkable();
    bool NeedsPierce();
    bool GoneAfterCombust();
    bool CapsExplosions();

    StaticTileData Data();
    
    void IncFireCount();
    void DecFireCount();
    
    void SubOnDestroy(Action cb);
    void UnsubOnDestroy(Action cb);

    void Initialise(bool isDropped);

    void Combust();
}