#include <iostream>
#define BUILD_WINDOWS
#include <Windows.h>

int main()
{
    OSVERSIONINFOW osv;
    osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFOW);
    if (GetVersionExW(&osv))
    {
        std::cout << "Windows Version: " << osv.dwMajorVersion << "." << osv.dwMinorVersion << "." << osv.dwBuildNumber << "\n";
    }
}
