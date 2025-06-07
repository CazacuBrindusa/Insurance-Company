using proiect_paw_2.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proiect_paw_2.Forms
{
    public partial class Event_Form : Form
    {
        private All alls { set; get; }

        private string ConectionString = "Data Source=database.db";

        public Event_Form()
        {
            alls = new All();
            InitializeComponent();
        }

        private void DisplayEvent()
        {
            listView1.Items.Clear();
            foreach(var even in alls.Events)
            {
                ListViewItem lvi = new ListViewItem(even.Event_Id.ToString());
                lvi.SubItems.Add(even.Client_Id.ToString());
                lvi.SubItems.Add(even.Insurance_Id.ToString());
                lvi.SubItems.Add(even.Name);
                lvi.SubItems.Add(even.Date.ToString());
                lvi.Tag = even;
                listView1.Items.Add(lvi);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add_Event_Form form = new Add_Event_Form();
            Event even = new Event();
            form.Event = even;
            if (form.ShowDialog() == DialogResult.OK)
            {
                //alls.Events.Add(even);
                AddEvent(even);
                DisplayEvent();
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*
            Add_Event_Form form = new Add_Event_Form();
            if (listView1.SelectedItems.Count == 1)
            {
                Event even = listView1.SelectedItems[0].Tag as Event;
                form.Event = even;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    DisplayEvent();
                }
            }
            */
            if (listView1.SelectedItems.Count == 1)
            {
                Event selectedEvent = listView1.SelectedItems[0].Tag as Event;
                if (selectedEvent != null)
                {
                    Add_Event_Form form = new Add_Event_Form
                    {
                        Event = selectedEvent
                    };

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        UpdateEvent(selectedEvent);
                        DisplayEvent();
                    }
                }
            }
        }

        private void UpdateEvent(Event even)
        {
            string query = "UPDATE Event SET Client_Id=@Client_Id, Insurance_Id=@Insurance_Id, Name = @Name, Date = @Date WHERE Event_Id = @Event_Id";
            using (SQLiteConnection connection = new SQLiteConnection(ConectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", even.Name);
                    command.Parameters.AddWithValue("@Date", even.Date);
                    command.Parameters.AddWithValue("@Client_Id", even.Client_Id);
                    command.Parameters.AddWithValue("@Insurance_Id", even.Insurance_Id);
                    command.Parameters.AddWithValue("@Event_Id", even.Event_Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void clientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Client_Form form = new Client_Form();
            form.ShowDialog();
        }

        private void insurancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Insurance_Form form = new Insurance_Form();
            form.ShowDialog();
        }

        private void AddEvent(Event even)
        {
            string query = "INSERT INTO Event(Client_Id, Insurance_Id, Date, Name) VALUES (@Client_Id, @Insurance_Id, @Date, @Name); " +
                "SELECT last_insert_rowid()";
            using (SQLiteConnection connection = new SQLiteConnection(ConectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.Parameters.AddWithValue("@Client_Id", even.Client_Id);
                command.Parameters.AddWithValue("@Insurance_Id", even.Insurance_Id);
                command.Parameters.AddWithValue("@Date", even.Date);
                command.Parameters.AddWithValue("@Name", even.Name);

                long id = (long)command.ExecuteScalar();
                even.Event_Id = (int)id;
                alls.Events.Add(even);
            }
        }

        private void LoadEvent()
        {
            string query = "SELECT * FROM Event";
            using (SQLiteConnection connection = new SQLiteConnection(ConectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int Event_Id = (int)(long)reader["Event_Id"];
                        int Client_Id = (int)(long)reader["Client_Id"];
                        int Insurance_Id = (int)(long)reader["Insurance_Id"];
                        string Name = (string)reader["Name"];
                        DateTime Date = DateTime.Parse((string)reader["Date"]);
                        Event even = new Event(Event_Id, Client_Id, Insurance_Id, Name, Date);
                        alls.Events.Add(even);
                    }
                }
            }
        }

        private void Event_Form_Load(object sender, EventArgs e)
        {
            LoadEvent();
            DisplayEvent();
        }

        private void DeleteEvent(Event even)
        {
            string query = "DELETE FROM Event WHERE Event_Id=@id";
            using (SQLiteConnection connection = new SQLiteConnection(ConectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@id", even.Event_Id);
                command.ExecuteNonQuery();
                alls.Events.Remove(even);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Choose a event!");
                return;
            }
            if (MessageBox.Show("Are you sure?", "Delete Event", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                ListViewItem item = listView1.SelectedItems[0];
                Event even = (Event)item.Tag;
                DeleteEvent(even);
                DisplayEvent();
            }
        }
    }
}
