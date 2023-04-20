// The MIT License (MIT)
// Windows Version Detection 1.1.0
// Copyright (C) 2022 - 2023 by Shao Voon Wong (shaovoon@yahoo.com)
//
// http://opensource.org/licenses/MIT

#include <iostream>
#include "WinVersion.h"

int main()
{
	VersionInfo info;
    if (WinVersion::GetVersion(info))
    {
        std::cout << "Windows Version: " << info.Major << "." << info.Minor << "." << info.BuildNum << "\n";
    }
	std::cout << "IsBuildNumGreaterOrEqual(Windows_11_22H2): " << std::boolalpha <<
        WinVersion::IsBuildNumGreaterOrEqual(BuildNumber::Windows_11_22H2) << "\n";
    return 0;
}
