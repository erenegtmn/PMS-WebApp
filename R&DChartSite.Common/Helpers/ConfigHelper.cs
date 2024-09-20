using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.Common.Helpers
{
    public class ConfigHelper
    {
        public static T Get<T>(string key)
        {
            var x = (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
            System.Diagnostics.Debug.WriteLine(x);
            return x;
            ;
        }
    }
}
