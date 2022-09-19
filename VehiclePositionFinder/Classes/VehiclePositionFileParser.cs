using VehiclePositionFinder.Models;

namespace VehiclePositionFinder.Classes
{
    internal class VehiclePositionFileParser
    {
        internal static IEnumerable<VehiclePosition>? Read(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"'{filePath}' doesn't exist!");
                return null;
            }

            var vehiclePositions = new List<VehiclePosition>();
            var data = File.ReadAllBytes(filePath);
            var offset = 0;

            while (offset < data.Length)
                vehiclePositions.Add(VehiclePosition.ReadBytes(data, ref offset));

            return vehiclePositions;
        }
    }
}