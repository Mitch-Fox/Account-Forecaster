using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Account_Forecaster
{
    public partial class MainForm : Form
    {
        private List<AccountingRowItem> AccountingRowItems = new List<AccountingRowItem>();
        private List<EndOfDayTotal> EndOfDayTotals = new List<EndOfDayTotal>();
        private const int NumberOfDaysToCalculateAhead = 365;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            dtpChartStartDate.MinDate = DateTime.Now;
            dtpChartStartDate.MaxDate = DateTime.Now.AddDays(NumberOfDaysToCalculateAhead);

            dtpChartEndDate.MinDate = DateTime.Now;
            dtpChartEndDate.MaxDate = DateTime.Now.AddDays(NumberOfDaysToCalculateAhead);

            cboFrequency.Items.AddRange(Frequency.AllFrequencies.ToArray());

            dgvAllItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvAllItems.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void btnAddNewItem_Click(object sender, EventArgs e)
        {
            var isIncome = false;
            if (cboIncomeOrExpense.SelectedItem.ToString() == "Income")
            {
                isIncome = true;
            }

            AccountingRowItems.Add(new AccountingRowItem()
            {
                Description = txtDescription.Text,
                IsIncome = isIncome,
                Amount = txtAmount.Text,
                Frequency = Frequency.GetFrequencyFromString(cboFrequency.SelectedItem.ToString()),
                DueDate = dtpDueDate.Value
            });

            RefreshDGV();
            RefreshCharts();
        }

        private void RefreshCharts()
        {
            RecalculateEndOfDayTotals();
            RepopulateChartSeries();
        }

        // X is time, Y is amount
        private void RepopulateChartSeries()
        {
            MainChart.Series.Clear();

            System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            };

            foreach (EndOfDayTotal endOfDayTotal in EndOfDayTotals)
            {
                series.Points.AddXY(endOfDayTotal.Date, endOfDayTotal.Total);
            }

            MainChart.Series.Add(series);

            MainChart.ChartAreas[0].RecalculateAxesScale();
        }

        private void RecalculateEndOfDayTotals()
        {
            EndOfDayTotals = new List<EndOfDayTotal>();

            if (decimal.TryParse(txtStartingBalance.Text, out decimal StartingBalance))
            {
                for (int i = 0; i < NumberOfDaysToCalculateAhead; i++)
                {
                    var newEndOfDayTotalDate = DateTime.Now.AddDays(i);
                    bool balanceHasChanged = false;

                    foreach (AccountingRowItem item in AccountingRowItems)
                    {
                        if (item.OccursOnGivenDay(newEndOfDayTotalDate))
                        {
                            if (item.IsIncome)
                            {
                                StartingBalance += item.AmountPerPayPeriod;
                            }
                            else
                            {
                                StartingBalance -= item.AmountPerPayPeriod;
                            }

                            balanceHasChanged = true;
                        }
                    }

                    if (balanceHasChanged)
                    {
                        EndOfDayTotals.Add(new EndOfDayTotal()
                        {
                            Date = newEndOfDayTotalDate,
                            Total = StartingBalance
                        });
                    }
                }
            }

        }

        private void RefreshDGV()
        {
            dgvAllItems.DataSource = null;
            dgvAllItems.DataSource = AccountingRowItems;
        }


        private void txtStartingBalance_Leave(object sender, EventArgs e)
        {
            RefreshCharts();
        }

        private void txtStartingBalance_TextChanged(object sender, EventArgs e)
        {
            RefreshCharts();
        }
    }
}
