using proiect_paw_2.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proiect_paw_2.Forms
{
    public partial class Add_Event_Form : Form
    {
        public Event Event { get; set; }

        public Add_Event_Form()
        {
            InitializeComponent();
        }

        private void Add_Event_Form_Load(object sender, EventArgs e)
        {
            if (Event != null)
            {
                numericUpDown2.Value = Event.Client_Id;
                numericUpDown3.Value = Event.Insurance_Id;
                textBox1.Text = Event.Name;
                dateTimePicker1.Value = Event.Date;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Event != null)
            {
                Event.Client_Id = (int)numericUpDown2.Value;
                Event.Insurance_Id = (int)numericUpDown3.Value;
                Event.Name = textBox1.Text;
                Event.Date = dateTimePicker1.Value;
            }
        }
    }
}
