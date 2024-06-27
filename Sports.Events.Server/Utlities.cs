using Sports.Events.Server.Models;

namespace Sports.Events.Server
{
    public class Utlities
    {
        private const double EarthRadiusMeters = 6378137; // Earth's radius in meters

        private static readonly Random random = new Random(); // Generates Random Number

        public static EventLocation GenerateRandomLocation(double centerLatitude, double centerLongitude, double maxDistanceMeters)
        {
            // Generate random offsets within the range of maxDistanceMeters
            double latOffset = (random.NextDouble() * 2 - 1) * (maxDistanceMeters / EarthRadiusMeters) * (180 / Math.PI);
            double lonOffset = (random.NextDouble() * 2 - 1) * (maxDistanceMeters / EarthRadiusMeters) * (180 / Math.PI) / Math.Cos(centerLatitude * Math.PI / 180);

            // Calculate new latitude and longitude
            double newLat = centerLatitude + latOffset;
            double newLon = centerLongitude + lonOffset;

            return new EventLocation { Latitude = newLat, Longitude = newLon };
        }

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Convert latitude and longitude from degrees to radians
            double dLat = DegreeToRadian(lat2 - lat1);
            double dLon = DegreeToRadian(lon2 - lon1);

            // Calculate the Haversine distance
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreeToRadian(lat1)) * Math.Cos(DegreeToRadian(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = EarthRadiusMeters * c;

            return distance;
        }

        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
