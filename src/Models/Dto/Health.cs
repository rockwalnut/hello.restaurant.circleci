using System;

using System.IO;
using System.Reflection;
using System.Globalization;
using System.Runtime.InteropServices;

namespace hello.restaurant.api.Models.DTO
{
    [Serializable]
    public class Health
    {
        public string Status { get; set; }
        public DateTime ServerDate { get; set; }
        public string FormattedServerDate { get; set; }
        public DateTime BuildDate { get; set; }
        public static string PATTERN_SERVER_DATE = "yyyy-MM-dd HH:mm:ss";

        public string Version { get; set; }
        public string Enviroment { get; set; }
        public string aspnetcore_enviroment { get; set; }

        public Health()
        {
            this.ServerDate = DateTime.UtcNow;
            this.FormattedServerDate = this.ServerDate.ToString(PATTERN_SERVER_DATE);
        }

        public Health(String status)
        {
            this.Status = status;
            this.ServerDate = DateTime.UtcNow;
            this.FormattedServerDate = this.ServerDate.ToString(PATTERN_SERVER_DATE);
            this.BuildDate = GetBuildDate(Assembly.GetExecutingAssembly());
            this.Version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion.ToString();

            // this.Enviroment = Environment.Configuration.Instance.GetEnvironment();

            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            this.aspnetcore_enviroment = env;
        }

        public DateTime GetLinkerTimestampUtc(Assembly assembly)
        {
            var location = assembly.Location;
            return GetLinkerTimestampUtc(location);
        }

        public DateTime GetLinkerTimestampUtc(string filePath)
        {
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            var bytes = new byte[2048];

            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                file.Read(bytes, 0, bytes.Length);
            }

            var headerPos = BitConverter.ToInt32(bytes, peHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(bytes, headerPos + linkerTimestampOffset);
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddSeconds(secondsSince1970);
        }

        private static DateTime GetBuildDate(Assembly assembly)
        {
            const string BuildVersionMetadataPrefix = "+build";

            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (attribute?.InformationalVersion != null)
            {
                var value = attribute.InformationalVersion;
                var index = value.IndexOf(BuildVersionMetadataPrefix);
                if (index > 0)
                {
                    value = value.Substring(index + BuildVersionMetadataPrefix.Length);
                    if (DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                    {
                        return result;
                    }
                }
            }

            return DateTime.MinValue;
        }
    }
}