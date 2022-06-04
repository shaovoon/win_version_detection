// The MIT License (MIT)
// Windows Version Detection 1.0.0
// Copyright (C) 2022 by Shao Voon Wong (shaovoon@yahoo.com)
//
// http://opensource.org/licenses/MIT

using win_version_csharp_net6;

WinVersion.GetVersion(out var info);
Console.WriteLine("Windows Version: {0}.{1}.{2}", info.Major, info.Minor, info.BuildNum);
Console.ReadKey();
