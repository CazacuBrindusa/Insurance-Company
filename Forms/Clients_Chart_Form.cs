using proiect_paw_2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace proiect_paw_2.Forms
{
    public partial class Clients_Chart_Form : Form
    {
        private List<Client> clients;

        public Clients_Chart_Form(List<Client> clients)
        {
            InitializeComponent();
            this.clients = clients;
            InitializeChart();
        }

        private void InitializeChart()
        {
            // Create a new Chart control
            Chart chart = new Chart
            {
                Dock = DockStyle.Fill,
                Name = "chart1"
            };
            this.Controls.Add(chart);

            // Create a ChartArea and add it to the chart
            ChartArea chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            // Create a Series and add it to the chart
            Series series = new Series
            {
                Name = "BirthYear",
                ChartType = SeriesChartType.Column
            };
            chart.Series.Add(series);
        }

        private void Clients_Chart_Form_Shown(object sender, EventArgs e)
        {
            LoadChartData();
        }

        public void LoadChartData()
        {
            Chart chart = this.Controls.OfType<Chart>().FirstOrDefault(c => c.Name == "chart1");
            if (chart != null)
            {
                // Retrieve the series by name
                Series series = chart.Series.FirstOrDefault(s => s.Name == "BirthYear");
                if (series != null)
                {
                    series.Points.Clear();

                    // Extract the year from each client's birthday and count occurrences
                    var yearCounts = clients.GroupBy(c => c.Birthday.Year)
                                            .Select(g => new { Year = g.Key, Count = g.Count() });

                    // Add data points for each year
                    foreach (var yearCount in yearCounts)
                    {
                        series.Points.AddXY(yearCount.Year, yearCount.Count);
                    }

                    // Customize chart appearance
                    chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1; // Show every year
                    chart.ChartAreas[0].AxisX.Title = "Year";
                    chart.ChartAreas[0].AxisY.Title = "Number of Clients";
                    chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Series 'BirthYear' not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
