using UnityEngine;

public interface IBaseView
{
    void ShowError(string message);

    void ShowSuccess(string message);

    void ShowLoading(bool show);
}