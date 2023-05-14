using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IDatabaseHandler
{
    Task<List<User>> GetUsersAsync();
    Task<SaveObject> GetSaveByIdAsync(int id);
    Task<User> GetUserByNameAsync(string name);
    Task<List<SaveObject>> GetAllSaveDataFromUser(int id);
    Task Init();
    Task<int> AddUserAsync(User user);
    Task<int> AddSaveDataAsync(SaveObject saveData);
    Task<int> EditSaveDataAsync(SaveObject saveData);
    Task<bool> UserExistsAsync(string userName);
    Task<Dictionary<int, GameData>> LoadAllProfiles(int currentUserId);
    Task Save(GameData gameData, int userId, int saveId);
    Task<int> CreateNewGame(GameData gameData, int userId);//return new Save Id
    Task<GameData> Load(int saveId);
}
