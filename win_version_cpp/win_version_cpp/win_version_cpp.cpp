// The MIT License (MIT)
// Windows Version Detection 1.0.0
// Copyright (C) 2022 by Shao Voon Wong (shaovoon@yahoo.com)
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
	std::cout << "IsBuildNumGreaterOrEqual(22000): " << WinVersion::IsBuildNumGreaterOrEqual(22000) << "\n";
    return 0;
}
