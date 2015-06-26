using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceContractFormsMgmt
{
    public class IOHelper
    {
        public static string GetStringFromStream(Stream stream)
        {
            string str;
            using (var sr = new StreamReader(stream))
            {
                str = sr.ReadToEnd();
            }
            return str;
        }
    }
}
