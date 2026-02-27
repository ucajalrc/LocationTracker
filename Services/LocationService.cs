using Microsoft.Maui.Devices.Sensors;

namespace LocationTracker.Services
{
    /// <summary>
    /// Responsible for retrieving device GPS location.
    /// </summary>
    public class LocationService
    {
        public async Task<Location?> GetCurrentLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(
                    GeolocationAccuracy.Best,
                    TimeSpan.FromSeconds(10));

                return await Geolocation.GetLocationAsync(request);
            }
            catch
            {
                return null;
            }
        }
    }
}