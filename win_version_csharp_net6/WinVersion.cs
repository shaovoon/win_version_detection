// The MIT License (MIT)
// Windows Version Detection 1.1.0
// Copyright (C) 2022 - 2023 by Shao Voon Wong (shaovoon@yahoo.com)
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

    public enum BuildNumber : uint
    {
        Windows_Vista = 6002,
        Windows_7 = 7601,
        Windows_8 = 9200,
        Windows_8_1 = 9600,
        Windows_10_1507 = 10240,
        Windows_10_1511 = 10586,
        Windows_10_1607 = 14393,
        Windows_10_1703 = 15063,
        Windows_10_1709 = 16299,
        Windows_10_1803 = 17134,
        Windows_10_1809 = 17763,
        Windows_10_1903 = 18362,
        Windows_10_1909 = 18363,
        Windows_10_2004 = 19041,
        Windows_10_20H2 = 19042,
        Windows_10_21H1 = 19043,
        Windows_10_21H2 = 19044,
        Windows_10_22H2 = 19045,
        Windows_11_21H2 = 22000,
        Windows_11_22H2 = 22621,
    };

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
            return info.BuildNum >= buildNumber;
        }

    }
}
