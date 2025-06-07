using proiect_paw_2.Entities;
using proiect_paw_2.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Drawing.Printing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace proiect_paw_2.Forms
{
    public partial class Client_Form : Form
    {
        private AllClientsViewModel viewModel;
        private string ConectionString = "Data Source=database.db";

        public Client_Form()
        {
            InitializeComponent();
            viewModel = new AllClientsViewModel();
            listView1.View = View.Details;
            listView1.Columns.Add("Client ID", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Name", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Birthday", -2, HorizontalAlignment.Left);
            listView1.FullRowSelect = true;

            listView1.MouseDoubleClick += listView1_MouseDoubleClick;
            LoadClients();
            DisplayClients();
            printDocument1.PrintPage += printDocument1_PrintPage;
        }

        private void DisplayClients()
        {
            listView1.Items.Clear();
            foreach (var client in viewModel.Clients)
            {
                ListViewItem lvi = new ListViewItem(client.Client_Id.ToString());
                lvi.SubItems.Add(client.Name);
                lvi.SubItems.Add(client.Birthday.ToString("yyyy-MM-dd"));
                lvi.Tag = client;
                listView1.Items.Add(lvi);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add_Client_Form form = new Add_Client_Form();
            ClientViewModel clientViewModel = new ClientViewModel(new Client());
            form.ViewModel = clientViewModel;
            if (form.ShowDialog() == DialogResult.OK)
            {
                AddClient(clientViewModel);
                viewModel.Clients.Add(clientViewModel);
                DisplayClients();
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ClientViewModel selectedClient = listView1.SelectedItems[0].Tag as ClientViewModel;
                if (selectedClient != null)
                {
                    Add_Client_Form form = new Add_Client_Form
                    {
                        ViewModel = selectedClient
                    };

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        UpdateClient(selectedClient);
                        DisplayClients();
                    }
                }
            }
        }

        private void serializeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = File.Create(sfd.FileName))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, viewModel.Clients);
                }
            }
        }

        private void deserializeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = File.OpenRead(ofd.FileName))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    var clients = (ObservableCollection<Client>)bf.Deserialize(fs);
                    viewModel.LoadClients(clients);
                    DisplayClients();
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.P))
            {
                ShowPrintPreview();
                return true; // Indicate that the key press was handled
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Define what to print
            if (viewModel.Clients != null && viewModel.Clients.Count > 0)
            {
                float yPos = 100;
                int count = 0;
                float leftMargin = e.MarginBounds.Left;
                float topMargin = e.MarginBounds.Top;

                foreach (var client in viewModel.Clients)
                {
                    string clientInfo = $"Client ID: {client.Client_Id}\nName: {client.Name}\nBirthday: {client.Birthday.ToShortDateString()}\n\n";
                    e.Graphics.DrawString(clientInfo, new Font("Arial", 12), Brushes.Black, leftMargin, yPos);
                    yPos += 80; // Adjust the line height as needed
                    count++;

                    // Check if more pages are needed
                    if (yPos + 80 > e.MarginBounds.Height)
                    {
                        e.HasMorePages = true;
                        return;
                    }
                }
                e.HasMorePages = false;
            }
        }

        private void ShowPrintPreview()
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clients_Chart_Form form = new Clients_Chart_Form(viewModel.Clients.Select(c => c._client).ToList());
            form.Shown += Clients_Chart_Form_Shown;
            form.ShowDialog();
        }

        private void Clients_Chart_Form_Shown(object sender, EventArgs e)
        {
            (sender as Clients_Chart_Form).LoadChartData();
        }

        private void AddClient(ClientViewModel clientViewModel)
        {
            string query = "INSERT INTO Client(Name, Birthday) VALUES (@Name, @Birthday); " +
                           "SELECT last_insert_rowid()";
            using (SQLiteConnection connection = new SQLiteConnection(ConectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@Name", clientViewModel.Name);
                command.Parameters.AddWithValue("@Birthday", clientViewModel.Birthday.ToString("yyyy-MM-dd"));
                long id = (long)command.ExecuteScalar();
                clientViewModel.Client_Id = (int)id;
            }
        }

        private void UpdateClient(ClientViewModel clientViewModel)
        {
            string query = "UPDATE Client SET Name = @Name, Birthday = @Birthday WHERE Client_Id = @Client_Id";
            using (SQLiteConnection connection = new SQLiteConnection(ConectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", clientViewModel.Name);
                    command.Parameters.AddWithValue("@Birthday", clientViewModel.Birthday.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@Client_Id", clientViewModel.Client_Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void LoadClients()
        {
            ObservableCollection<Client> clients = new ObservableCollection<Client>();
            string query = "SELECT * FROM Client";
            using (SQLiteConnection connection = new SQLiteConnection(ConectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int Client_Id = (int)(long)reader["Client_Id"];
                        string Name = (string)reader["Name"];
                        DateTime Birthday = DateTime.Parse((string)reader["Birthday"]);
                        Client client = new Client(Client_Id, Name, Birthday);
                        clients.Add(client);
                    }
                }
            }
            viewModel.LoadClients(clients);
        }

        private void Client_Form_Load(object sender, EventArgs e)
        {
            LoadClients();
            DisplayClients();
        }

        private void DeleteClient(ClientViewModel clientViewModel)
        {
            string query = "DELETE FROM Client WHERE Client_Id=@id";
            using (SQLiteConnection connection = new SQLiteConnection(ConectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@id", clientViewModel.Client_Id);
                command.ExecuteNonQuery();
                viewModel.Clients.Remove(clientViewModel);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Choose a client!");
                return;
            }
            if (MessageBox.Show("Are you sure?", "Delete client", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                ListViewItem item = listView1.SelectedItems[0];
                ClientViewModel clientViewModel = (ClientViewModel)item.Tag;
                DeleteClient(clientViewModel);
                DisplayClients();
            }
        }
    }
}
