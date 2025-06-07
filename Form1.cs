using proiect_paw_2.Entities;
using proiect_paw_2.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proiect_paw_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client_Form form = new Client_Form();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Insurance_Form form = new Insurance_Form();
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Event_Form form = new Event_Form();
            form.ShowDialog();
        }


    }
}
