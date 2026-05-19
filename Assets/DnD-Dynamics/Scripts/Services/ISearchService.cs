using System.Collections.Generic;

public interface ISearchService
{
    List<Item> SearchByName(List<Item> items, string query);
    List<Item> SearchByDescription(List<Item> items, string query);
    List<Item> SearchByKeyword(List<Item> items, string keyword);
}