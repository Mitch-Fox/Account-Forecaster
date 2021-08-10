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
using System.Windows.Forms.DataVisualization.Charting;

namespace Account_Forecaster
{
    public partial class MainForm : Form
    {
        private List<AccountingRowItem> AccountingRowItems = new List<AccountingRowItem>();
        private List<EndOfDayTotal> EndOfDayTotals = new List<EndOfDayTotal>();
        private int DefaultNumberOfDaysToCalculate = 30;

        private TimeSpan NumberOfDaysToCalculate => dtpChartEndDate.Value - dtpChartStartDate.Value;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var startDate = DateTime.Now;
            dtpChartStartDate.MinDate = startDate;
            dtpChartStartDate.Value = startDate;

            var endDate = startDate.AddDays(DefaultNumberOfDaysToCalculate);
            dtpChartEndDate.MinDate = endDate;
            dtpChartEndDate.Value = endDate;

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

            Series series = new Series { ChartType = SeriesChartType.Line };

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
                for (int i = 0; i < NumberOfDaysToCalculate.TotalDays; i++)
                {
                    var nextDate = dtpChartStartDate.Value.AddDays(i);
                    bool balanceHasChanged = false;

                    foreach (AccountingRowItem item in AccountingRowItems)
                    {
                        if (item.OccursOnGivenDay(nextDate))
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
                            Date = nextDate,
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

        private void dtpChartStartDate_ValueChanged(object sender, EventArgs e)
        {
            RefreshCharts();
        }

        private void dtpChartEndDate_ValueChanged(object sender, EventArgs e)
        {
            RefreshCharts();
        }
    }
}
