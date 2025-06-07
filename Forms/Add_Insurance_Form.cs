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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace proiect_paw_2.Forms
{
    public partial class Add_Insurance_Form : Form
    {
        public Insurance Insurance { get; set; }

        public Add_Insurance_Form()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (Insurance != null)
            {
                Insurance.Sum = (int)numericUpDown2.Value;
                Insurance.Start_Date = dateTimePicker1.Value;
                Insurance.End_Date = dateTimePicker2.Value;
            }
        }

        private void Add_Insurance_Form_Load(object sender, EventArgs e)
        {
            if (Insurance != null)
            {
                numericUpDown2.Value = Insurance.Sum;
                dateTimePicker1.Value = Insurance.Start_Date;
                dateTimePicker2.Value = Insurance.End_Date;
            }
        }
    }
}
