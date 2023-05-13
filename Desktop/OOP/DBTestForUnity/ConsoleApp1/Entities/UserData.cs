using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Entities
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
    [Table("Saves")]
    public class Save
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        public int LevelNum { get; set; }
        public int CharacterLives { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public string HeartsCollected { get; set; }
        public int UserId { get; set; }
    }
}
