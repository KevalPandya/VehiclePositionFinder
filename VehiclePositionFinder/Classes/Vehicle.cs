using GeoCoordinatePortable;
using Geohash;
using System.Diagnostics;
using VehiclePositionFinder.Models;

namespace VehiclePositionFinder.Classes
{
    internal class Vehicle
    {
        internal static IEnumerable<VehiclePosition>? Find(params GeoCoordinate[] listOfCurrentLocations)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var listOfVehicles = VehiclePositionFileParser.Read("VehiclePositions.dat");
            stopwatch.Stop();
            long fileReadDuartion = stopwatch.ElapsedMilliseconds;

            if (listOfVehicles == null)
                return null;

            stopwatch.Restart();
            var nearestVehicles = GetNearestVehicle(listOfVehicles, listOfCurrentLocations);
            stopwatch.Stop();

            Console.WriteLine($"Data file read execution time: {fileReadDuartion} ms");
            Console.WriteLine($"Closest position calculation execution time: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Total execution time: {fileReadDuartion + stopwatch.ElapsedMilliseconds} ms");

            return nearestVehicles;
        }

        private static IEnumerable<VehiclePosition> GetNearestVehicle(IEnumerable<VehiclePosition> listOfVehicles, GeoCoordinate[] listOfCurrentLocations)
        {
            var nearestVehicles = new List<VehiclePosition>();

            Parallel.ForEach(listOfCurrentLocations, currentLocation =>
            {
                var currentLocationGeohash = new Geohasher().Encode(currentLocation.Latitude, currentLocation.Longitude);

                var vehicle = listOfVehicles
                        .Where(v => v.VehicleGeohash == currentLocationGeohash)
                        .OrderBy(v => currentLocation.GetDistanceTo(new GeoCoordinate(v.VehicleLatitude, v.VehicleLongitude)))
                        .First();

                nearestVehicles.Add(vehicle);
            });

            return nearestVehicles;
        }
    }
}