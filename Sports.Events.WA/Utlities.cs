using Sports.Events.WA.Models;

namespace Sports.Events.WA
{
    /// <summary>
    /// Utility class providing common functions for event-related operations.
    /// </summary>
    public class Utlities
    {
        /// <summary>
        /// Azure Maps subscription key used for accessing map services.
        /// </summary>
        public static string subscriptionKey = "EuspMNDTUpoG6xTDX5qJbIrkvz3ijjxVrDlqRbpnqJ6TLxQNf3dKJQQJ99AFACYeBjF9k1EiAAAgAZMPje1D";

        /// <summary>
        /// Syncfusion subscription key.
        /// </summary>
        public static string syncfusionKey = "Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXpceHRSRmddVEx1X0s=";

        private const double EarthRadiusMeters = 6378137; // Earth's radius in meters

        private static readonly Random random = new Random(); // Generates Random Number

        /// <summary>
        /// Generates a random location near the specified center coordinates within a certain distance range.
        /// </summary>
        /// <param name="centerLatitude">Latitude of the center point.</param>
        /// <param name="centerLongitude">Longitude of the center point.</param>
        /// <param name="maxDistanceMeters">Maximum distance from the center point in meters.</param>
        /// <returns>A randomly generated <see cref="EventLocation"/>.</returns>
        public static EventLocation GenerateRandomLocation(double centerLatitude, double centerLongitude, double maxDistanceMeters)
        {
            double latOffset = (random.NextDouble() * 2 - 1) * (maxDistanceMeters / EarthRadiusMeters) * (180 / Math.PI);
            double lonOffset = (random.NextDouble() * 2 - 1) * (maxDistanceMeters / EarthRadiusMeters) * (180 / Math.PI) / Math.Cos(centerLatitude * Math.PI / 180);

            double newLat = centerLatitude + latOffset;
            double newLon = centerLongitude + lonOffset;

            return new EventLocation { Latitude = newLat, Longitude = newLon };
        }

        /// <summary>
        /// Calculates the distance between two geographical coordinates using the Haversine formula.
        /// </summary>
        /// <param name="lat1">Latitude of the first point.</param>
        /// <param name="lon1">Longitude of the first point.</param>
        /// <param name="lat2">Latitude of the second point.</param>
        /// <param name="lon2">Longitude of the second point.</param>
        /// <returns>The distance between the two points in meters.</returns>
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double dLat = DegreeToRadian(lat2 - lat1);
            double dLon = DegreeToRadian(lon2 - lon1);

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
