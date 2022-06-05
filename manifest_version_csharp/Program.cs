using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manifest_version_csharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var info = Environment.OSVersion.Version;
            Console.WriteLine("Windows Version: {0}.{1}.{2}", info.Major, info.Minor, info.Build);
        }
    }
}
