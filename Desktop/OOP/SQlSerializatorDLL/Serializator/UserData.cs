using SQLite;

namespace Serializator
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
    public class SaveObject
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        public int LevelNum { get; set; }
        public int CharacterLives { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public string HeartsCollected { get; set; }
        public int UserId { get; set; }
    }

}
