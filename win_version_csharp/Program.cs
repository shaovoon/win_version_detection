// The MIT License (MIT)
// Windows Version Detection 1.0.0
// Copyright (C) 2022 by Shao Voon Wong (shaovoon@yahoo.com)
//
// http://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace win_version_csharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VersionInfo info = new VersionInfo();
            if (WinVersion.GetVersion(out info))
            {
                Console.WriteLine("Windows Version: {0}.{1}.{2}", info.Major, info.Minor, info.BuildNum);
                Console.ReadKey();
            }
        }
    }
}
