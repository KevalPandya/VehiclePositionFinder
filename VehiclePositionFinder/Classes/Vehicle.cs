using GeoCoordinatePortable;
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
                var nearestVehicle = new VehiclePosition
                {
                    UserLatitude = currentLocation.Latitude.ToString(),
                    UserLongitude = currentLocation.Longitude.ToString()
                };

                foreach (var vehicle in listOfVehicles)
                {
                    var vehicleCoordinate = new GeoCoordinate(vehicle.VehicleLatitude, vehicle.VehicleLongitude);
                    var calculatedDistance = currentLocation.GetDistanceTo(vehicleCoordinate);

                    if (calculatedDistance < nearestVehicle.Distance || nearestVehicle.Distance == null)
                    {
                        nearestVehicle.PositionId = vehicle.PositionId;
                        nearestVehicle.VehicleRegistration = vehicle.VehicleRegistration;
                        nearestVehicle.VehicleLatitude = vehicle.VehicleLatitude;
                        nearestVehicle.VehicleLongitude = vehicle.VehicleLongitude;
                        nearestVehicle.RecordedTimeUTC = vehicle.RecordedTimeUTC;
                        nearestVehicle.Distance = calculatedDistance;
                    }
                }

                nearestVehicles.Add(nearestVehicle);
            });

            return nearestVehicles;
        }
    }
}