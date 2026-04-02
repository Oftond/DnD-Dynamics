using System.Collections.Generic;

public interface IDataService
{
    void Save<T>(string key, T data);

    T Load<T>(string key, T defaultValue = default);

    bool Exists(string key);

    void Delete(string key);

    List<string> GetAllKeys();
}