// The MIT License (MIT)
// Windows Version Detection 1.0.0
// Copyright (C) 2022 by Shao Voon Wong (shaovoon@yahoo.com)
//
// http://opensource.org/licenses/MIT

using System;
using System.Runtime.InteropServices;

namespace win_version_csharp_net6
{
    public struct VersionInfo
    {
        public int Major;
        public int Minor;
        public int BuildNum;
    }

    public class WinVersion
    {

        public static void GetVersion(out VersionInfo info)
        {
            info.Major = Environment.OSVersion.Version.Major;
            info.Minor = Environment.OSVersion.Version.Minor;
            info.BuildNum = Environment.OSVersion.Version.Build;
            if (info.BuildNum >= 22000)
                info.Major = 11;
        }
    }
}
