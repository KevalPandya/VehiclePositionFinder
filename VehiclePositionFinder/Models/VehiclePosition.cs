using System.Text;

namespace VehiclePositionFinder.Models
{
    internal class VehiclePosition
    {
        public int PositionId;

        public string VehicleRegistration;

        public float VehicleLatitude;

        public float VehicleLongitude;

        public DateTime RecordedTimeUTC;

        public string UserLatitude;

        public string UserLongitude;

        public double? Distance;

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

            return vehiclePosition;
        }
    }
}