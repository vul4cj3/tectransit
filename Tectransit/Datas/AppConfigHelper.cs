using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Tectransit.Datas
{
    //get appsettings.json data
    public class AppConfigHelper
    {
        public readonly string _connectionString = string.Empty;
        public AppConfigHelper()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            var root = configurationBuilder.Build();

            _connectionString = root.GetSection("ConnectionString").GetSection("tecConn").Value;
            
        }
        public string ConnectionString
        {
            get => _connectionString;
        }


    }
}
