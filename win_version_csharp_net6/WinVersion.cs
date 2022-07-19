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
            var version = Environment.OSVersion.Version;
            info.Major = version.Major;
            info.Minor = version.Minor;
            info.BuildNum = version.Build;
            if (info.BuildNum >= 22000)
                info.Major = 11;
        }
        public static bool IsBuildNumGreaterOrEqual(uint buildNumber)
        {
            GetVersion(out var info);
            return buildNumber >= info.BuildNum;
        }

    }
}
