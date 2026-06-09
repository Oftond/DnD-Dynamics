using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class PortraitDataService : IPortraitDataService
{
    private readonly Dictionary<string, Texture2D> _cache = new();
    private readonly HashSet<string> _loadingPaths = new();
    private readonly int _maxTextureSize;

    public event Action<string, Texture2D> OnPortraitLoaded;

    public PortraitDataService(int maxTextureSize = 512)
    {
        _maxTextureSize = maxTextureSize;
    }

    public Texture2D GetPortrait(string path)
    {
        if (string.IsNullOrEmpty(path))
            return null;

        if (!File.Exists(path))
        {
            Debug.LogWarning($"[PortraitCache] Файл не найден: {path}");

            return null;
        }

        if (_cache.TryGetValue(path, out var cached))
            return cached;

        if (_loadingPaths.Contains(path))
            return null;

        _loadingPaths.Add(path);
        LoadPortraitAsync(path);

        return null;
    }

    private async void LoadPortraitAsync(string path)
    {
        try
        {
            // ✅ ШАГ 1: Читаем байты файла в ФОНОВОМ потоке (I/O операция)
            byte[] fileData = await Task.Run(() => File.ReadAllBytes(path));

            // ✅ ШАГ 2: Создаём текстуру в ГЛАВНОМ потоке (требование Unity)
            var texture = CreateTextureFromBytes(fileData);

            if (texture != null)
            {
                // ✅ ШАГ 3: Ресайз тоже в главном потоке
                if (texture.width > _maxTextureSize || texture.height > _maxTextureSize)
                    texture = ResizeTexture(texture, _maxTextureSize);

                _cache[path] = texture;
                OnPortraitLoaded?.Invoke(path, texture);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[PortraitCache] Ошибка загрузки: {ex.Message}");
        }
        finally
        {
            _loadingPaths.Remove(path);
        }



        //try
        //{
        //    var texture = await Task.Run(() => LoadTextureFromFile(path));

        //    if (texture != null)
        //    {
        //        _cache[path] = texture;
        //        OnPortraitLoaded?.Invoke(path, texture);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Debug.LogError($"[PortraitCache] Ошибка загрузки: {ex.Message}");
        //}
        //finally
        //{
        //    _loadingPaths.Remove(path);
        //}
    }

    private Texture2D CreateTextureFromBytes(byte[] fileData)
    {
        try
        {
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);

            if (!texture.LoadImage(fileData))
            {
                Debug.LogError("[PortraitCache] Не удалось декодировать изображение");
                UnityEngine.Object.Destroy(texture);
                return null;
            }

            return texture;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[PortraitCache] Ошибка создания текстуры: {ex.Message}");
            return null;
        }
    }

    private Texture2D LoadTextureFromFile(string path)
    {
        try
        {
            byte[] fileData = File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2);

            if (!texture.LoadImage(fileData))
            {
                Debug.LogError($"[PortraitCache] Не удалось декодировать изображение: {path}");
                return null;
            }

            if (texture.width > _maxTextureSize || texture.height > _maxTextureSize)
                texture = ResizeTexture(texture, _maxTextureSize);

            return texture;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[PortraitCache] Ошибка чтения файла: {ex.Message}");
            return null;
        }
    }

    private Texture2D ResizeTexture(Texture2D source, int maxSize)
    {
        float ratio = Mathf.Min((float)maxSize / source.width, (float)maxSize / source.height);

        int newWidth = Mathf.RoundToInt(source.width * ratio);
        int newHeight = Mathf.RoundToInt(source.height * ratio);

        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        Graphics.Blit(source, rt);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D resized = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false);
        resized.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        resized.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return resized;
    }

    public Sprite CreateSpriteFromTexture(Texture2D texture)
    {
        if (texture == null)
            return null;

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void RemovePortrait(string path)
    {
        if (string.IsNullOrEmpty(path)) return;

        if (_cache.TryGetValue(path, out var texture))
        {
            if (texture != null)
                UnityEngine.Object.Destroy(texture);
            _cache.Remove(path);
        }

        if (File.Exists(path))
        {
            try
            {
                File.Delete(path);
                Debug.Log($"[PortraitCache] Удалён файл: {path}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[PortraitCache] Ошибка удаления: {ex.Message}");
            }
        }
    }

    public void PreloadPortrait(string path) => GetPortrait(path);

    public void ClearCache()
    {
        foreach (var texture in _cache.Values)
        {
            if (texture != null)
                UnityEngine.Object.Destroy(texture);
        }

        _cache.Clear();
    }
}