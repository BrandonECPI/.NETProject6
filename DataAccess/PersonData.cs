using SQLite;
using MAUI_Data_Access.Models;

namespace MAUI_Data_Access.DataAccess
{
    /// <summary>
    /// Data access layer: reads and saves Person records in SQLite.
    /// </summary>
    public class PersonData
    {
        private SQLiteAsyncConnection? database;

        // Opens the database and creates the table the first time it's needed
        private async Task Init()
        {
            if (database is not null)
            {
                return;
            }

            database = new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
            await database.CreateTableAsync<Person>();
        }

        // Returns every person in the database
        public async Task<List<Person>> GetPeopleAsync()
        {
            await Init();
            return await database!.Table<Person>().ToListAsync();
        }

        // Returns one person by ID (not used in this app, example only)
        public async Task<Person> GetPersonAsync(int id)
        {
            await Init();
            return await database!.Table<Person>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        // Adds a new person, or updates one that already has an ID
        public async Task<int> SavePersonAsync(Person person)
        {
            await Init();

            if (person.ID != 0)
            {
                // Update an existing person
                return await database!.UpdateAsync(person);
            }
            else
            {
                // Save a new person
                return await database!.InsertAsync(person);
            }
        }

        // Deletes a person (not used in this app, example only)
        public async Task<int> DeletePersonAsync(Person person)
        {
            await Init();
            return await database!.DeleteAsync(person);
        }
    }
}