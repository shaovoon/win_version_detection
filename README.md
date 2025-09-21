# Windows 11 Version Detection

## Table of Contents

* Introduction
* Windows Vista Fiasco
* Manifest Method
* RtlGetVersion
* Windows Version Retrieval Hack
* Reference
* History

## Introduction

![winver_table.png](/images/winver_table.png)

No mistake in the above table! Windows 11 version is indeed `10.0`. From the Windows kernel perspective, Windows 10 and 11 are largely the same. The Windows 11 naming is perhaps a marketing decision.

## Windows Vista Fiasco

Many programs written for Windows XP, refused to run on Vista due to the check below. Vista version is `6.0` in which the `version.Minor` check will fail because `0` is neither greater than nor equal to `1`. Windows 7 version is `6.1` so it is not affected by it.

```Cpp
if (version.Major >= 5 && version.Minor >= 1) 
{
    // WinXP or later: good to go!?
}
```

<p id="corrected_xp">The corrected check is below but was too late to fix the Windows XP applications out there during Vista era.

```Cpp
if (version.Major > 5 || 
   (version.Major == 5 && version.Minor >= 1) )
{
    // WinXP or later
}
```

To prevent history from repeating itself, Microsoft devised another method of querying the Windows version based on an XML-format manifest file for .NET Framework 4.8 and C++ respectively. The manifest file is embedded in the final executable. .NET6 does not need this manifest file to obtain the correct Windows version. Sadly, it does seem .NET Framework 4.8 and C++ projects are left behind by Microsoft in favor of .NET6.

## Manifest Method

In this section, we&#39;ll use the code below to explore the XML manifest method to query the Windows version. Click on the C++ tab to see C++ source code if you&#39;re a C++ programmer.


```CSharp
using System;

var info = Environment.OSVersion.Version;
Console.WriteLine("Windows Version: {0}.{1}.{2}", 
                  info.Major, info.Minor, info.Build);
```

```Cpp
#include <Windows.h>

OSVERSIONINFOW osv;
osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFOW);
if (GetVersionExW(&osv))
{
    std::cout << "Windows Version: "
              << osv.dwMajorVersion << "." 
              << osv.dwMinorVersion << "." 
              << osv.dwBuildNumber << "\n";
}
```

This is the version outputted by the above code on a Windows 11 machine. `6.2` is the version of Windows 8.0. Clearly, this is incorrect. Let&#39;s add a manifest file to the project to fix it.

```
Windows Version: 6.2.9200
```

In the Visual Studio solution, right-click on the C# project name in __Solution Explorer__ and select __Add->New Item__. A selection dialog comes up and click __General__ and select the __Application Manifest File (Windows Only)__ and enter its filename as _app.manifest_.

![cs_add_manifest.png](/images/cs_add_manifest.png)

For Visual C++, Microsoft did not provide a way to add a manifest file. We&#39;ll do this instead: copy the _app.manifest_ from the C# project to the Visual C++ project folder and rename it to _manifest.xml_ and follow these steps to inform Visual C++ of this _manifest.xml_. Right-click on the C++ project in the __Solution Explorer__ and select __Properties__ and in the __Properties Dialog__ that pops up and click __Manifest Tool->Input and Output->Additional Manifest Files__ and add "_manifest.xml_".

![cpp_add_manifest.png](/images/cpp_add_manifest.png)

In the manifest file, we&#39;ll uncomment the `supportedOS` for Windows 8.1 by deleting the surrounding `<!--` and `-->` and see its effect.

```xml
<!-- Windows 8.1 -->
<!-- <supportedOS Id="{1f676c76-80e1-4239-95bb-83d0f6d0da78}" /> -->

<!-- Windows 10 -->
<!-- <supportedOS Id="{8e0f7a12-bfb3-4fe8-b9a5-48fd50a15a9a}" /> -->
```

Now the code outputs `6.3` which is Windows 8.1 which is still incorrect. Remember my machine is installed with Windows 11.

```
Windows Version: 6.3.9600
```

Next, we&#39;ll uncomment the `supportedOS` for Windows 10 to see its effect.

```
Windows Version: 10.0.22000
```

Now, this is correct. Windows 10 and 11 share the same major and minor version and Windows 11 is differentiated by its build number, `22000`. The manifest method ensures that the application never gets a version higher than what is specified in its manifest. Next, we&#39;ll use a Windows kernel function `RtlGetVersion` to bypass the manifest file to always report the current Windows version without affecting by the presence/absence of manifest.

## RtlGetVersion

In our `GetVersion`, we&#39;ll call `RtlGetVersion` via P/Invoke. For C++ programmers, please click on the C++ tab to see the C++ source code.

```CSharp
public static bool GetVersion(out VersionInfo info)
{
    info.Major = 0;
    info.Minor = 0;
    info.BuildNum = 0;
    OSVERSIONINFOEXW osv = new OSVERSIONINFOEXW();
    osv.dwOSVersionInfoSize = 284;
    if (RtlGetVersion(out osv) == 0)
    {
        info.Major = osv.dwMajorVersion;
        info.Minor = osv.dwMinorVersion;
        info.BuildNum = osv.dwBuildNumber;

        return true;
    }
    return false;
}
```

```Cpp
bool WinVersion::GetVersion(VersionInfo& info)
{
	OSVERSIONINFOEXW osv;
	osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFOEXW);
	if (RtlGetVersion(&osv) == 0)
	{
		info.Major = osv.dwMajorVersion;
		info.Minor = osv.dwMinorVersion;
		info.BuildNum = osv.dwBuildNumber;

		return true;
	}
	return false;
}
```

This is how the `GetVersion()` is used.

```CSharp
if (WinVersion.GetVersion(out var info))
{
    Console.WriteLine("Windows Version: {0}.{1}.{2}", 
                      info.Major, info.Minor, info.BuildNum);
    Console.ReadKey();
}
```

```Cpp
VersionInfo info;
if (WinVersion::GetVersion(info))
{
    std::cout << "Windows Version : " 
              << info.Major << "." 
              << info.Minor << "." 
              << info.BuildNum << "\n";
}
```

This is the Windows 11 version.

```
Windows Version: 10.0.22000
```

## Windows Version Retrieval Hack

In the [Windows Native API Programming](https://leanpub.com/windowsnativeapiprogramming) book by Pavel Yosifovich, it is mentioned a method to get the Windows version through the `KUSER_SHARED_DATA` struct located at `0x7ffe0000` address in every process. For more information about `KUSER_SHARED_DATA`, read this [MSDN](https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntddk/ns-ntddk-kuser_shared_data) link. `KUSER_SHARED_DATA` is located in `<ntddk.h>` that comes with Windows Driver Development Kit (DDK). You must install the DDK to compile the code snippet below.

```Cpp
// Requires installation of the Windows Driver Development Kit (ddk)
#include <ntddk.h>
#include <cstdio>

int main()
{
    auto data = (KUSER_SHARED_DATA*)0x7ffe0000;
    printf("Version: %d.%d.%d\n",
    data->NtMajorVersion, data->NtMinorVersion, data->NtBuildNumber);

    return 0;
}
```

Since I do not have DDK installed on my system. Fortunately, I found this code snippet in [Windows 10 System Programming, Part 1](https://leanpub.com/windows10systemprogramming) book, also written by same author. This code does not rely on `KUSER_SHARED_DATA` but the address offsets to its structure members. It compiles and works fine on my Visual C++ 2022.

```Cpp
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <cstdio>

int main()
{
    auto sharedUserData = (BYTE*)0x7FFE0000;
    printf("Version: %d.%d.%d\n",
        *(ULONG*)(sharedUserData + 0x26c),  // major version offset
        *(ULONG*)(sharedUserData + 0x270),  // minor version offset
        *(ULONG*)(sharedUserData + 0x260)); // build number offset

    return 0;
}
```

The program output is 10.0.22621 which is correct on my Windows 11 22H2.

```
Version: 10.0.22621
```

__Update__: I completed my 32-bit and 64-bit testing of this hack. On Windows XP, Vista, 7, 8 and 8.1, the build number is an invalid zero while it is valid on Windows 10 and 11. Just beware not to rely on the build number on the Windows OS older than 10 if you use this hack.

## References

* [Wikipedia: List of Microsoft Windows versions](https://en.wikipedia.org/wiki/List_of_Microsoft_Windows_versions)
* [MSDN Q&A: Win32 API to detect Windows 11](https://docs.microsoft.com/en-us/answers/questions/547050/win32-api-to-detect-windows-11.html)
* [MSDN Q&A: SupportedOS ID for Windows 11](https://docs.microsoft.com/en-us/answers/questions/464971/supportedos-id-for-windows-11.html)
* [Pluralsight: Windows 11 Internals: Foundations](https://app.pluralsight.com/library/courses/windows-11-internals-foundations/table-of-contents) by Pavel Yosifovich

## History

* 2<sup>nd</sup> September, 2023: Remove the Windows 11 build number check to set major version to 11.
* 27<sup>th</sup> August, 2023: Added testing results of 32-bit and 64-bit Windows OSes to Windows Version Retrieval Hack</a> section
* 30<sup>th</sup> July, 2023: Added Windows Version Retrieval Hack</a> section
* 20<sup>th</sup> April, 2023: Fixed `IsBuildNumGreaterOrEqual()` bug and added Windows `BuildNumber enum` for `IsBuildNumGreaterOrEqual()`
* 30<sup>th</sup> August, 2022: Added `IsBuildNumGreaterOrEqual()` method in the source code for users who like to detect version via build number
* 5<sup>th</sup> July, 2022: Added the corrected Windows XP check
* 3<sup>rd</sup> July, 2022: First release
