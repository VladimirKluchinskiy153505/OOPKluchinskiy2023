using ConsoleApp1.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace ConsoleApp1.Services
{
    public interface IDbService
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByNameAsync(string name);
        Task<List<Save>> GetAllSaveDataFromUser(int id);
        Task Init();
        Task<int> AddUserAsync(User user);
        Task<int> AddSaveDataAsync(Save saveData);
        Task<int> EditSaveDataAsync(Save saveData);

        Task<bool> UserExistsAsync(string userName);
    }
    public class Database : IDbService
    {
        public SQLiteAsyncConnection database;
        public static string dbPath = Path.Combine("C:\\Users\\avdot\\Desktop\\OOP\\SQLDataBase","UnityGame.db3");
        public async Task Init()
        {
            File.WriteAllText(dbPath, string.Empty);
            database = new SQLiteAsyncConnection(dbPath);
            await database.CreateTableAsync<User>();
            await database.CreateTableAsync<Save>();
        }
        public async Task<List<User>> GetUsersAsync()
        {
            if (database == null) { await Init(); }
            return await database.Table<User>().ToListAsync();
        }
        public async Task<User> GetUserByNameAsync(string userName)
        {
            if (database == null) { await Init(); }
            foreach(User user in await this.GetUsersAsync())
            {
                if(user.Name == userName)
                {
                    return user;
                }
            }
            return null;
        }
        public async Task<List<Save>> GetAllSaveDataFromUser(int id)
        {
            if (database == null) { await Init(); }
            List<Save> saveList = await database.Table<Save>().ToListAsync();
            List<Save> needList = new List<Save>();
            foreach (Save save in saveList)
            {
                if (save.UserId== id)
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
        public async Task<int> AddSaveDataAsync(Save saveData)
        {
            if (database == null) { await Init(); }
            return await database.InsertAsync(saveData);
        }

        public async Task<int> EditSaveDataAsync(Save saveData)
        {
            if (database == null) { await Init(); }
            return await database.UpdateAsync(saveData);
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            if (database == null) { await Init(); }
            foreach(User user in await this.GetUsersAsync())
            {
                if (user.Name == userName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
