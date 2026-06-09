using System;
using UnityEngine;

public interface IPortraitDataService
{
    event Action<string, Texture2D> OnPortraitLoaded;

    Texture2D GetPortrait(string path);

    Sprite CreateSpriteFromTexture(Texture2D texture);

    void RemovePortrait(string path);

    void PreloadPortrait(string path);

    void ClearCache();
}