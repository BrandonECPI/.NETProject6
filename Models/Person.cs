using SQLite;

namespace MAUI_Data_Access.Models
{
    /// <summary>
    /// A person saved in the People table.
    /// </summary>
    public class Person
    {
        // Unique ID; SQLite fills this in automatically
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DoB { get; set; }
    }
}