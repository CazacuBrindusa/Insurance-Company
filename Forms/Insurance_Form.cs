using proiect_paw_2.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proiect_paw_2.Forms
{
    public partial class Insurance_Form : Form
    {
        private All alls { set; get; }

        private string ConectionString = "Data Source=database.db";

        public Insurance_Form()
        {
            alls = new All();
            InitializeComponent();
        }

        private void DisplayInsurance()
        {
            listView1.Items.Clear();
            foreach (var insurance in alls.Insurances)
            {
                ListViewItem lvi = new ListViewItem(insurance.Insurance_Id.ToString());
                lvi.SubItems.Add(insurance.Start_Date.ToString());
                lvi.SubItems.Add(insurance.End_Date.ToString());
                lvi.SubItems.Add(insurance.Sum.ToString());
                lvi.Tag = insurance;
                listView1.Items.Add(lvi);
            }
        }

        private void AddInsurance(Insurance insurance)
        {
            string query = "INSERT INTO Insurance(Start_Date, End_Date, Sum) VALUES (@Start_Date, @End_Date, @Sum); " +
                "SELECT last_insert_rowid()";
            using (SQLiteConnection connection = new SQLiteConnection(ConectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.Parameters.AddWithValue("@Start_Date", insurance.Start_Date);
                command.Parameters.AddWithValue("@End_Date", insurance.End_Date);
                command.Parameters.AddWithValue("@Sum", insurance.Sum);

                long id=(long)command.ExecuteScalar();
                insurance.Insurance_Id = (int)id;
                alls.Insurances.Add(insurance);
            }
        }

        private void LoadInsurances()
        {
            string query = "SELECT * FROM Insurance";
            using (SQLiteConnection connection = new SQLiteConnection(ConectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int Insurance_Id = (int)(long)reader["Insurance_Id"];
                        DateTime Start_Date = DateTime.Parse((string)reader["Start_Date"]);
                        DateTime End_Date = DateTime.Parse((string)reader["End_Date"]);
                        int Sum = (int)(long)reader["Sum"];
                        Insurance insurance = new Insurance(Insurance_Id, Start_Date, End_Date, Sum);
                        alls.Insurances.Add(insurance);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add_Insurance_Form form = new Add_Insurance_Form();
            Insurance insurance = new Insurance();
            form.Insurance= insurance;
            if (form.ShowDialog() == DialogResult.OK)
            {
                //alls.Insurances.Add(insurance);
                AddInsurance(insurance);
                DisplayInsurance();
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*
            Add_Insurance_Form form = new Add_Insurance_Form();
            if (listView1.SelectedItems.Count == 1)
            {
                Insurance insurance = listView1.SelectedItems[0].Tag as Insurance;
                form.Insurance = insurance;
                if (form.ShowDialog() == DialogResult.OK)
                {   
                    DisplayInsurance();
                }
            }
            */
            if (listView1.SelectedItems.Count == 1)
            {
                Insurance selectedInsurance = listView1.SelectedItems[0].Tag as Insurance;
                if (selectedInsurance != null)
                {
                    Add_Insurance_Form form = new Add_Insurance_Form
                    {
                        Insurance = selectedInsurance
                    };

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        UpdateInsurance(selectedInsurance);
                        DisplayInsurance();
                    }
                }
            }
        }

        private void UpdateInsurance(Insurance insurance)
        {
            string query = "UPDATE Insurance SET Start_Date = @Start_Date, End_Date = @End_Date, Sum=@Sum WHERE Insurance_Id = @Insurance_Id";
            using (SQLiteConnection connection = new SQLiteConnection(ConectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Start_Date", insurance.Start_Date);
                    command.Parameters.AddWithValue("@End_Date", insurance.End_Date);
                    command.Parameters.AddWithValue("@Sum", insurance.Sum);
                    command.Parameters.AddWithValue("@Insurance_Id", insurance.Insurance_Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text File|*.txt";
            saveFileDialog.Title = "Save All Insurances Report";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    writer.WriteLine("All Insurances Report");
                    writer.WriteLine("---------------------");
                    foreach (var insurance in alls.Insurances)
                    {
                        writer.WriteLine($"Insurance ID: {insurance.Insurance_Id}");
                        writer.WriteLine($"Start Date: {insurance.Start_Date.ToShortDateString()}");
                        writer.WriteLine($"End Date: {insurance.End_Date.ToShortDateString()}");
                        writer.WriteLine($"Sum: {insurance.Sum}");
                        writer.WriteLine();
                    }
                }
                MessageBox.Show("Report saved successfully.");
            }
        }

        private void Insurance_Form_Load(object sender, EventArgs e)
        {
            LoadInsurances();
            DisplayInsurance();
        }

        private void DeleteInsurance(Insurance insurance)
        {
            string query = "DELETE FROM Insurance WHERE Insurance_Id=@id";
            using (SQLiteConnection connection = new SQLiteConnection(ConectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@id", insurance.Insurance_Id);
                command.ExecuteNonQuery();
                alls.Insurances.Remove(insurance);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Choose a insurance!");
                return;
            }
            if(MessageBox.Show("Are you sure?", "Delete Insurance", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                ListViewItem item = listView1.SelectedItems[0];
                Insurance insurance = (Insurance)item.Tag;
                DeleteInsurance(insurance);
                DisplayInsurance();
            }
        }
    }
}
