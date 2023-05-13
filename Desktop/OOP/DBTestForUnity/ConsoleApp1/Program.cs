using ConsoleApp1.Entities;
using ConsoleApp1.Services;
using System.Runtime.CompilerServices;
using System.Text.Json;

public class Program
{
   static IDbService service;
    static async Task Main()
    {
        Console.WriteLine("hello");
        service = new Database();
        await SeedData();

        await SeeUsersAsync();
        Console.WriteLine();
        await SeeUserSavesAsync(1);
        await service.EditSaveDataAsync(new Save
        {
            Id = 1,
            LevelNum = 2,
            CharacterLives = 3,
            X = 1, Y = 2,Z = 3,
            HeartsCollected = JsonSerializer.Serialize(new Dictionary<string, bool>()
            {
                {"0000", true},
                {"1111", false}
            }),
            UserId = 1
        });
        Console.WriteLine();
        await SeeUserSavesAsync(1);
       // await SingUp();
        await SeeUsersAsync();
        await SeeUserSavesAsync(10);
        await LogIn();
    }
    public static async Task SeeUsersAsync()
    {
        var userList = await service.GetUsersAsync();
        foreach (var user in userList)
        {
            Console.WriteLine("ID: " + user.Id + " Name: " + user.Name + " Password: " + user.Password);
        }
    }
    public static async Task SeeUserSavesAsync(int userId)
    {
        var saveList = await service.GetAllSaveDataFromUser(userId);
        foreach (var save in saveList)
        {
            Console.WriteLine("saveId: "+save.Id +" Dict: "+ save.HeartsCollected+" UserId: "+ save.UserId);
        }
    }
    public static async Task SingUp()
    {
        string userName;
        bool nameTaken = true, nameEmpty;
        do
        {
            Console.WriteLine("Enter UserName");
            userName = Console.ReadLine();
            nameEmpty = (userName == null)|| (userName == string.Empty);
            if (nameEmpty)
            {
                Console.WriteLine("Incorrect name, try again");
                continue;
            }
            nameTaken = await service.UserExistsAsync(userName);
            if (nameTaken)
            {
                Console.WriteLine("This name is already taken, enter another");
            }
        } while (nameTaken);

        string password, repeatPassword;
        bool passwordMismatch=true, emptyPassword;
        do
        {
            Console.WriteLine("Create Password");
            password = Console.ReadLine();
            emptyPassword = (password == null) || (password == string.Empty);
            if (emptyPassword)
            {
                Console.WriteLine("Incorrent password, try again");
                continue;
            }
            Console.WriteLine("Confirm your password");
            repeatPassword = Console.ReadLine();
            passwordMismatch = (password != repeatPassword);
            if (passwordMismatch)
            {
                Console.WriteLine("mismatch in password, try again");
            }
        }while(passwordMismatch);
        await service.AddUserAsync(new User
        {
            Name = userName,
            Password = password
        });
    }
    public static async Task LogIn()
    {
        string userName, password;
        bool emptyName, emptyPassword;
        bool correctPassword = false;
        int userId=0;
        do
        {
            await Console.Out.WriteLineAsync("Enter UserName");
            userName = Console.ReadLine();
            await Console.Out.WriteLineAsync("Enter Password");
            password = Console.ReadLine();
            emptyName = (userName == null) || (userName == string.Empty);
            emptyPassword = (password == null) || (password == string.Empty);
            if (emptyName || emptyPassword)
            {
                Console.WriteLine("Password or UserName Are Incorrect");
                continue;
            }
            User user = await service.GetUserByNameAsync(userName);
            if (user == null)
            {
                Console.WriteLine("Password or UserName Are Incorrect");
                continue;
            }
            correctPassword = (user.Password == password);
            if (correctPassword)
            {
                userId = user.Id;
            }
        } while (!correctPassword);
        await Console.Out.WriteLineAsync("You are entered");
        await SeeUserSavesAsync(userId);
    }
    public static async Task SeedData()
    {
        await service.AddUserAsync(new User
        {
            Name = "Oleg",
            Password = "1111"
        });
        await service.AddUserAsync(new User
        {
            Name = "Ivan",
            Password = "2222"
        });
        await service.AddUserAsync(new User
        {
            Name = "Vlad",
            Password = "3333"
        });
        await service.AddUserAsync(new User
        {
            Name = "Max",
            Password = "4444"
        });
        //////////////////////////////////////////
        await service.AddSaveDataAsync(new Save
        {
            LevelNum = 1,
            CharacterLives = 1,
            X = 0,
            Y = 0,
            Z = 0,
            HeartsCollected = JsonSerializer.Serialize(new Dictionary<string, bool>()
            {
                {"1111", true},
                {"2222", false}
            }),
            UserId = 1
        });
        await service.AddSaveDataAsync(new Save
        {
            LevelNum = 3,
            CharacterLives = 4,
            X = 1,
            Y = 1,
            Z = 1,
            HeartsCollected = JsonSerializer.Serialize(new Dictionary<string, bool>()
            {
                {"2222", true},
                {"3333", false}
            }),
            UserId = 1
        });
        await service.AddSaveDataAsync(new Save
        {
            LevelNum = 2,
            CharacterLives = 44,
            X = 1,
            Y = 1,
            Z = 1,
            HeartsCollected = JsonSerializer.Serialize(new Dictionary<string, bool>()
            {
                {"5555", true},
                {"3333", false}
            }),
            UserId = 2
        });
    } 
}