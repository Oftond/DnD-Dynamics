using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class SearchService : ISearchService
{
    public List<Item> SearchByName(List<Item> items, string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return items;

        string lowerQuery = query.ToLower();

        return items.Where(item => item.Name.ToLower().Contains(lowerQuery)).ToList();
    }

    public List<Item> SearchByDescription(List<Item> items, string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return items;

        string lowerQuery = query.ToLower();

        return items.Where(item => item.Description != null && item.Description.ToLower().Contains(lowerQuery)).ToList();
    }

    public List<Item> SearchByKeyword(List<Item> items, string keyword)
    {
        var byName = SearchByName(items, keyword);
        var byDesc = SearchByDescription(items, keyword);

        return byName.Union(byDesc).ToList();
    }
}