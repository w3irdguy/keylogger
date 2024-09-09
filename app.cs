using System;
using System.Windows.Forms;

namespace FakeSecurityApp
{
    public class MainForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;

        public MainForm()
        {
            // Configure the form
            this.Text = "Fake Security System";
            this.Size = new System.Drawing.Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Username label and textbox
            var lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new System.Drawing.Point(20, 20);
            lblUsername.AutoSize = true;
            this.Controls.Add(lblUsername);

            txtUsername = new TextBox();
            txtUsername.Location = new System.Drawing.Point(100, 20);
            txtUsername.Width = 150;
            this.Controls.Add(txtUsername);

            // Password label and textbox
            var lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Location = new System.Drawing.Point(20, 60);
            lblPassword.AutoSize = true;
            this.Controls.Add(lblPassword);

            txtPassword = new TextBox();
            txtPassword.Location = new System.Drawing.Point(100, 60);
            txtPassword.Width = 150;
            txtPassword.UseSystemPasswordChar = true;
            this.Controls.Add(txtPassword);

            // Login button
            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new System.Drawing.Point(100, 100);
            btnLogin.Click += new EventHandler(OnLoginClick);
            this.Controls.Add(btnLogin);
        }

        private void OnLoginClick(object sender, EventArgs e)
        {
            MessageBox.Show("Login button clicked!");
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
