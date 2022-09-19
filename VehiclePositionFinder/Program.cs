using GeoCoordinatePortable;
using VehiclePositionFinder.Classes;

var nearestVehicles = Vehicle.Find(
    new GeoCoordinate(34.544909, -102.100843),
    new GeoCoordinate(32.345544, -99.123124),
    new GeoCoordinate(33.234235, -100.214124),
    new GeoCoordinate(35.195739, -95.348899),
    new GeoCoordinate(31.895839, -97.789573),
    new GeoCoordinate(32.895839, -101.789573),
    new GeoCoordinate(34.115839, -100.225732),
    new GeoCoordinate(32.335839, -99.992232),
    new GeoCoordinate(33.535339, -94.792232),
    new GeoCoordinate(32.234235, -100.222222)
);

if (nearestVehicles != null)
    foreach (var vehicle in nearestVehicles)
    {
        Console.WriteLine($"\nUser Location: {vehicle.UserLatitude}, {vehicle.UserLongitude}");
        Console.WriteLine($"Position Id: {vehicle.PositionId}");
        Console.WriteLine($"Vehicle Registration: {vehicle.VehicleRegistration}");
        Console.WriteLine($"Vehicle Location: {vehicle.VehicleLatitude}, {vehicle.VehicleLongitude}");
        Console.WriteLine($"RecordedTimeUTC: {vehicle.RecordedTimeUTC}");
        Console.WriteLine($"Distance: {Math.Round(Convert.ToDecimal(vehicle.Distance))} m");
    }