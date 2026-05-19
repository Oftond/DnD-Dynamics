using System.Collections.Generic;
using UnityEditor;

public interface IHandbookView
{
    void DisplayItems(List<HandbookEntity> items);
    void DisplayDetails(HandbookEntity item);
    void ShowLoading(bool show);
    void ShowError(string message);
    void ShowSuccess(string message);
    void ClearSelection();
}