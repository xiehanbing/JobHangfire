using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Jobs.Core.Common
{
    public class ApiConfig
    {
        public static string HangfireLogUrl ="";
        /// <summary>
        /// JobDbConnectionName
        /// </summary>
        public const string JobDbConnectionName = "JobConnection";
        /// <summary>
        /// GeneralConnectionName
        /// </summary>
        public const string GeneralConnectionName = "DefaultConnection";
    }
}