using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using LocationTracker.Data;

namespace LocationTracker;

public partial class MainPage : ContentPage
{
	private readonly LocationDatabase _database;

	public MainPage(LocationDatabase database)
	{
		InitializeComponent();
		_database = database;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
		if (status != PermissionStatus.Granted)
		{
			await DisplayAlert("Permission Required", "Location access is required for tracking.", "OK");
			return;
		}

		var oldEntries = await _database.GetLocationsAsync();
		foreach (var entry in oldEntries)
			await _database.DeleteLocationAsync(entry); // Create this method in your DB class

		StartTracking();

		// Load heatmap
		var entries = await _database.GetLocationsAsync();
		UpdateHeatMap(entries);
	}


	private void StartTracking()
	{
		Dispatcher.StartTimer(TimeSpan.FromMinutes(30), () =>
		{
			_ = TrackLocationAsync();
			return true; // keep timer running
		});
	}

	private async Task TrackLocationAsync()
	{
		try
		{
			// Get current location of the user 
			var location = await Geolocation.GetLocationAsync(new GeolocationRequest
			{
				DesiredAccuracy = GeolocationAccuracy.High,
				Timeout = TimeSpan.FromSeconds(10)
			});

			if (location != null)
			{
				var entry = new LocationEntry
				{
					Latitude = location.Latitude,
					Longitude = location.Longitude,
					Timestamp = DateTime.Now
				};

				await _database.SaveLocationAsync(entry);

				var entries = await _database.GetLocationsAsync();

				MainThread.BeginInvokeOnMainThread(() =>
				{
					UpdateHeatMap(entries);
				});
			}
		}
		catch (Exception ex)
		{
			// Handle or log error
			Console.WriteLine($"Location error: {ex.Message}");
		}
	}

	private void UpdateHeatMap(List<LocationEntry> entries)
	{
		map.MapElements.Clear();
		map.Pins.Clear();

		if (entries == null || entries.Count == 0)
			return;

		// Add circles for each location 
		foreach (var e in entries)
		{
			var circle = new Circle
			{
				Center = new Location(e.Latitude, e.Longitude),
				Radius = Distance.FromMeters(50),
				StrokeWidth = 1,
				FillColor = new Color(65 / 255f, 105 / 255f, 225 / 255f, 1f)
			};
			map.MapElements.Add(circle);
		}

		var latest = entries.LastOrDefault();
		if (latest != null)
		{
			map.MoveToRegion(MapSpan.FromCenterAndRadius(
				new Location(latest.Latitude, latest.Longitude),
				Distance.FromMeters(500)
			));
		}
	}
}