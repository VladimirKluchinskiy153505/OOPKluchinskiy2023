using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using SQLite;

public class DatabaseHandler : IDatabaseHandler
{
    public SQLiteAsyncConnection database;
    public static string dbPath = Path.Combine("C:\\Users\\avdot\\Desktop\\OOP\\SQLDataBase", "UnityGame1.db3");
    public async Task Init()
    {
        //File.WriteAllText(dbPath, string.Empty);
        database = new SQLiteAsyncConnection(dbPath);
        await database.CreateTableAsync<User>();
        await database.CreateTableAsync<SaveObject>();
    }
    public async Task<List<User>> GetUsersAsync()
    {
        if (database == null) { await Init(); }
        return await database.Table<User>().ToListAsync();
    }
    public async Task<User> GetUserByNameAsync(string userName)
    {
        if (database == null) { await Init(); }
        foreach (User user in await this.GetUsersAsync())
        {
            if (user.Name == userName)
            {
                return user;
            }
        }
        return null;
    }
    public async Task<SaveObject> GetSaveByIdAsync(int id)
    {
        if (database == null) { await Init(); }
        return await database.FindAsync<SaveObject>(id);
    }
    public async Task<List<SaveObject>> GetAllSaveDataFromUser(int id)
    {
        if (database == null) { await Init(); }
        List<SaveObject> saveList = await database.Table<SaveObject>().ToListAsync();
        List<SaveObject> needList = new List<SaveObject>();
        foreach (SaveObject save in saveList)
        {
            if (save.UserId == id)
            {
                needList.Add(save);
            }
        }
        return needList;
    }
    public async Task<int> AddUserAsync(User user)
    {
        if (database == null) { await Init(); }
        return await database.InsertAsync(user);
    }
    public async Task<int> AddSaveDataAsync(SaveObject saveData)
    {
        if (database == null) { await Init(); }
        return await database.InsertAsync(saveData);
    }

    public async Task<int> EditSaveDataAsync(SaveObject saveData)
    {
        if (database == null) { await Init(); }
        return await database.UpdateAsync(saveData);
    }

    public async Task<bool> UserExistsAsync(string userName)
    {
        if (database == null) { await Init(); }
        foreach (User user in await this.GetUsersAsync())
        {
            if (user.Name == userName)
            {
                return true;
            }
        }
        return false;
    }
    public async Task<Dictionary<int, GameData>> LoadAllProfiles(int currentUserId)
    {
        Dictionary<int,GameData> profileDictionary = new Dictionary<int, GameData>();
        List<SaveObject> saves = await this.GetAllSaveDataFromUser(currentUserId);
        foreach (SaveObject save in saves)
        {

            profileDictionary.Add(save.Id, new GameData
            {
                levelNum = save.LevelNum,
                lastUpdated = 0,
                playerPosition = new Vector3(save.X, save.Y, save.Z),
                heartsCollected = JsonUtility.FromJson<Dictionary<string, bool>>(save.HeartsCollected),
                characterLives = save.CharacterLives

            });
        }
        return profileDictionary;
    }

    public async Task Save(GameData gameData, int userId, int saveId)
    {
        Debug.Log("HandlerSave method:" + "UserId: " + userId + " SaveId " + saveId+" playerPosition "+gameData.playerPosition.ToString());
        await this.EditSaveDataAsync(new SaveObject
        {
            Id = saveId,
            LevelNum =gameData.levelNum,
            CharacterLives =gameData.characterLives,
            X = gameData.playerPosition.x,
            Y = gameData.playerPosition.y,
            Z = gameData.playerPosition.z,
            HeartsCollected = JsonUtility.ToJson(gameData.heartsCollected),
            UserId = userId,

        });
    }
    public async Task<int> CreateNewGame(GameData gameData, int userId)
    {
        Debug.Log("Create New Game method");
        await this.AddSaveDataAsync(new SaveObject
        {
            LevelNum = gameData.levelNum,
            CharacterLives = gameData.characterLives,
            X = gameData.playerPosition.x,
            Y = gameData.playerPosition.y,
            Z = gameData.playerPosition.z,
            HeartsCollected = JsonUtility.ToJson(gameData.heartsCollected),
            UserId = userId,
        });
       int newId = await database.ExecuteScalarAsync<int>("SELECT last_insert_rowid()");
       Debug.Log($"NewId {newId}");
       return newId;
    }
    public async Task<GameData> Load(int saveId)
    {
        Debug.Log("HandlerLoad method saveId: " + saveId);
        SaveObject save = await this.GetSaveByIdAsync(saveId);
        return new GameData
        {
            levelNum = save.LevelNum,
            lastUpdated = 0,
            playerPosition = new Vector3(save.X, save.Y, save.Z),
            heartsCollected = JsonUtility.FromJson<Dictionary<string, bool>>(save.HeartsCollected),
            characterLives = save.CharacterLives
        };
    }

}
