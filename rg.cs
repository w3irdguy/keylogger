using System;
using System.Drawing;
using System.Windows.Forms;

namespace FakeAntivirus
{
    public class MainForm : Form
    {
        private Button btnOk;
        private ProgressBar progressBar;
        private System.Windows.Forms.Timer timer; // Especifica que estamos usando o Timer do System.Windows.Forms
        private bool isProcessing;

        public MainForm()
        {
            // Configure the form
            this.Text = "MamasBoy AntiVirus";
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Text label
            var lblMessage = new Label();
            lblMessage.Text = "O dispositivo está infectado com um vírus. Aperte 'OK' para removê-lo.";
            lblMessage.Location = new Point(20, 20);
            lblMessage.Size = new Size(360, 40);
            lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblMessage);

            // OK button
            btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.Location = new Point(150, 80);
            btnOk.Click += new EventHandler(OnOkClick);
            this.Controls.Add(btnOk);

            // Progress bar
            progressBar = new ProgressBar();
            progressBar.Location = new Point(20, 120);
            progressBar.Size = new Size(340, 30);
            progressBar.Maximum = 100;
            this.Controls.Add(progressBar);

            // Timer
            timer = new System.Windows.Forms.Timer(); // Usa o Timer do System.Windows.Forms
            timer.Interval = 60000; // 1 minuto
            timer.Tick += new EventHandler(OnTimerTick);
        }
        private void OnOkClick(object sender, EventArgs e)
        {
            // Disable the OK button and start processing
            btnOk.Enabled = false;
            StartProcessing();
        }

        private void StartProcessing()
        {
            // Simulate opening a port (for educational purposes only)
            // OpenPort(23);

            isProcessing = true;
            progressBar.Value = 0;
            timer.Start();
            MessageBox.Show("O programa está agindo. Não feche o programa.", "Ação em andamento", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (progressBar.Value < 100)
            {
                progressBar.Value += 1;
            }
            else
            {
                timer.Stop();
                isProcessing = false;
                MessageBox.Show("O vírus foi removido com sucesso!", "Conclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Simulate closing the port (for educational purposes only)
                // ClosePort(23);

                this.Close();
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        // Simulated methods for opening and closing ports
        // These methods do nothing and are commented out for safety
        
        private void OpenPort(int port)
        {
            try
            {
                TcpListener listener = new TcpListener(System.Net.IPAddress.Any, port);
                listener.Start();
                // Port is open
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir a porta: " + ex.Message);
            }
        }

        private void ClosePort(int port)
        {
            // Close the port if it was opened
        }
    }
}
