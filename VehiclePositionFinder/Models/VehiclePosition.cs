using Geohash;
using System.Text;

namespace VehiclePositionFinder.Models
{
    internal class VehiclePosition
    {
        private static readonly Geohasher hasher = new();

        public int PositionId;

        public string VehicleRegistration;

        public float VehicleLatitude;

        public float VehicleLongitude;

        public DateTime RecordedTimeUTC;

        public string VehicleGeohash;

        internal static VehiclePosition ReadBytes(byte[] buffer, ref int offset)
        {
            var vehiclePosition = new VehiclePosition();

            vehiclePosition.PositionId = BitConverter.ToInt32(buffer, offset);
            offset += 4;

            var stringBuilder = new StringBuilder();
            while (buffer[offset] != 0)
            {
                stringBuilder.Append((char)buffer[offset]);
                offset++;
            }

            vehiclePosition.VehicleRegistration = stringBuilder.ToString();
            offset++;

            vehiclePosition.VehicleLatitude = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            vehiclePosition.VehicleLongitude = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            ulong time = BitConverter.ToUInt64(buffer, offset);
            vehiclePosition.RecordedTimeUTC = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(time);
            offset += 8;

            vehiclePosition.VehicleGeohash = hasher.Encode(vehiclePosition.VehicleLatitude, vehiclePosition.VehicleLongitude);

            return vehiclePosition;
        }
    }
}