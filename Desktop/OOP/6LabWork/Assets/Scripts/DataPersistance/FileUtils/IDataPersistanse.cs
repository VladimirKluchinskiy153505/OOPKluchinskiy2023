using Serializator;

public interface IDataPersistance
{
    void LoadData(GameData data);
    void SaveData(GameData data);
}
