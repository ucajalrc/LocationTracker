using SQLite;

namespace LocationTracker.Data;

public class LocationDatabase
{
	private readonly SQLiteAsyncConnection _database;

	public LocationDatabase(string dbPath)
	{
		//database connection to SQLite database
		_database = new SQLiteAsyncConnection(dbPath);

		//create table if it doesn't exist for location entries
		_database.CreateTableAsync<LocationEntry>().Wait();
	}

	public Task<int> SaveLocationAsync(LocationEntry location) =>
		_database.InsertAsync(location);


	// grab all location entries
	public Task<List<LocationEntry>> GetLocationsAsync() =>
		_database.Table<LocationEntry>().ToListAsync();

	// Delete a location entry
	public Task<int> DeleteLocationAsync(LocationEntry location) =>
	_database.DeleteAsync(location);

}