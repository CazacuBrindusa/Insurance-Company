using proiect_paw_2.Entities;
using proiect_paw_2.ViewModels;
using System;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;

namespace proiect_paw_2.Forms
{
    public partial class Add_Client_Form : Form
    {
        public ClientViewModel ViewModel { get; set; }
        private int progressStep;

        public Add_Client_Form()
        {
            InitializeComponent();
            // Initialize the ProgressBar
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            progressBar1.Value = 0;

            // Initialize the Timer
            timer1.Interval = 100; // 100 milliseconds
            timer1.Tick += new EventHandler(timer1_Tick);

            // Enable drag and drop for textBox1
            textBox1.AllowDrop = true;
            textBox1.DragEnter += new DragEventHandler(textBox1_DragEnter);
            textBox1.DragDrop += new DragEventHandler(textBox1_DragDrop);
        }

        private void Add_Client_Form_Load(object sender, EventArgs e)
        {
            if (ViewModel != null)
            {
                textBox1.DataBindings.Add("Text", ViewModel, "Name");
                dateTimePicker1.DataBindings.Add("Value", ViewModel, "Birthday");
                UpdateProgressBar(); // Update progress on load
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && dateTimePicker1.Value <= DateTime.Now)
            {
                // Start the progress bar animation
                progressStep = 0;
                progressBar1.Value = 0;
                timer1.Start();

                DialogResult = DialogResult.OK; // Set DialogResult to OK
                Close(); // Close the form
            }
            else
            {
                MessageBox.Show("Please enter a valid name and date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressStep < 20)
            {
                progressBar1.Value += 5;
                progressStep++;
            }
            else
            {
                timer1.Stop();
            }
        }

        private void dateTimePicker1_Validating(object sender, CancelEventArgs e)
        {
            DateTime selectedDate = dateTimePicker1.Value;
            if (selectedDate > DateTime.Now)
            {
                e.Cancel = true;
                errorProvider1.SetError(dateTimePicker1, "Invalid birthday");
            }
            else
            {
                errorProvider1.SetError(dateTimePicker1, ""); // Clear error if valid
            }
        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {
            // Reset the form fields and progress bar
            textBox1.Clear();
            dateTimePicker1.Value = DateTime.Now;
            progressBar1.Value = 0;

            if (ViewModel != null)
            {
                ViewModel.Client_Id = 0;
                ViewModel.Name = string.Empty;
                ViewModel.Birthday = DateTime.Now;
            }
        }

        // Method to update the ProgressBar value based on form field completion
        private void UpdateProgressBar()
        {
            int progress = 0;

            if (!string.IsNullOrEmpty(textBox1.Text)) progress += 50; // Assuming Name field is 50% of the form
            if (dateTimePicker1.Value != DateTime.MinValue && dateTimePicker1.Value <= DateTime.Now) progress += 50; // Assuming Birthday field is 50% of the form

            progressBar1.Value = progress;
        }

        // Optional: Handle events to update the progress bar dynamically
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateProgressBar();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UpdateProgressBar();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            UpdateProgressBar();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.S))
            {
                button1.PerformClick(); // Simulate button click
                return true; // Indicate that the key press was handled
            }
            if (keyData == (Keys.Alt | Keys.P))
            {
                ShowPrintPreview();
                return true; // Indicate that the key press was handled
            }
            if (keyData == (Keys.Control | Keys.C) && ViewModel != null)
            {
                CopyClientToClipboard(ViewModel);
                return true;
            }
            if (keyData == (Keys.Control | Keys.V))
            {
                PasteClientFromClipboard();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Define what to print
            if (ViewModel != null)
            {
                string clientInfo = $"Client ID: {ViewModel.Client_Id}\nName: {ViewModel.Name}\nBirthday: {ViewModel.Birthday.ToShortDateString()}";
                e.Graphics.DrawString(clientInfo, new Font("Arial", 12), Brushes.Black, new PointF(100, 100));
            }
        }

        private void ShowPrintPreview()
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // Handle potential future enhancements or additional functionality
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                textBox1.Text = (string)e.Data.GetData(DataFormats.Text);
            }
        }

        private void CopyClientToClipboard(ClientViewModel clientViewModel)
        {
            string clientData = $"Client ID: {clientViewModel.Client_Id}\nName: {clientViewModel.Name}\nBirthday: {clientViewModel.Birthday.ToShortDateString()}";
            Clipboard.SetText(clientData);
        }

        private void PasteClientFromClipboard()
        {
            if (Clipboard.ContainsText())
            {
                string clipboardText = Clipboard.GetText();
                string[] clientInfo = clipboardText.Split('\n');

                if (clientInfo.Length >= 3)
                {
                    ClientViewModel clientViewModel = new ClientViewModel(new Client());
                    clientViewModel.Client_Id = int.Parse(clientInfo[0].Split(':')[1].Trim());
                    clientViewModel.Name = clientInfo[1].Split(':')[1].Trim();
                    clientViewModel.Birthday = DateTime.Parse(clientInfo[2].Split(':')[1].Trim());

                    // Update the form fields
                    textBox1.Text = clientViewModel.Name;
                    dateTimePicker1.Value = clientViewModel.Birthday;

                    // Update the ViewModel property
                    ViewModel = clientViewModel;
                }
            }
        }

        private void Add_Client_Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                if (Clipboard.ContainsText())
                {
                    string clipboardText = Clipboard.GetText();
                    string[] parts = clipboardText.Split('\t');
                    if (parts.Length >= 2)
                    {
                        textBox1.Text = parts[0];
                        dateTimePicker1.Text = parts[1];
                    }
                }
            }
        }
    }
}
