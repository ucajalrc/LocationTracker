using SQLite;
using LocationTracker.Models;

namespace LocationTracker.Services
{
    /// <summary>
    /// Handles all SQLite database operations.
    /// </summary>
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);

            // Create table if it doesn't exist
            _database.CreateTableAsync<LocationEntry>().Wait();
        }

        /// <summary>
        /// Saves a new location entry into database.
        /// </summary>
        public Task<int> SaveLocationAsync(LocationEntry location)
        {
            return _database.InsertAsync(location);
        }

        /// <summary>
        /// Retrieves all saved locations.
        /// </summary>
        public Task<List<LocationEntry>> GetLocationsAsync()
        {
            return _database.Table<LocationEntry>().ToListAsync();
        }
    }
}