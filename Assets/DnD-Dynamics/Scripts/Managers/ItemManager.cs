using System.Collections.Generic;
using UnityEngine;

public static class ItemManager
{
    private static List<ItemData> _allItemsData;

    public static List<ItemData> GetAllItemsData()
    {
        if (_allItemsData == null || _allItemsData.Count == 0)
            _allItemsData = GameDataService.Instance.LoadItems();

        return _allItemsData;
    }

    public static ItemData GetItemDataById(string id)
    {
        var items = GetAllItemsData();

        return items.Find(i => i.Id == id);
    }

    public static List<ItemData> GetItemsByType(string type) => GameDataService.Instance.GetItemsByType(type);

    public static Item CreateItemFromData(string itemId)
    {
        var itemData = GetItemDataById(itemId);
        if (itemData == null) return null;

        var item = new Item();
        item.SetData(itemData);

        return item;
    }
}