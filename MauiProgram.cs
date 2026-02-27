using LocationTracker;
using LocationTracker.Data;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{

		//create the app builder
		var builder = MauiApp.CreateBuilder();

		//configure the app
		builder
			.UseMauiApp<App>() //main application class
			.UseMauiMaps() //map functionality
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); //add custom font
			});

		// local SQLite database path
		string dbPath = Path.Combine(FileSystem.AppDataDirectory, "locations.db3");

		builder.Services.AddSingleton(new LocationDatabase(dbPath));

		return builder.Build();
	}

}