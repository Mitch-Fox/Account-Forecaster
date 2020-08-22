using System;
using System.Globalization;
using NodaTime;

namespace Account_Forecaster
{
    public class AccountingRowItem
    {
        public string Description { get; set; }
        public bool IsIncome { get; set; }
        public Frequency Frequency { get; set; }
        //public Category category;
        //public CostType costType;
        public decimal AmountPerPayPeriod;
        public DateTime DueDate { get; set; }

        public string Amount
        {
            get
            {
                return AmountPerPayPeriod.ToString("C", CultureInfo.CurrentCulture);
            }
            set
            {
                decimal.TryParse(value, out AmountPerPayPeriod);
            }
        }

        public string CostPerMonth
        {
            get
            {
                decimal costPerMonth = AmountPerPayPeriod * Frequency.NumberOfOccurrencesPerYear / 12;
                return costPerMonth.ToString("C", CultureInfo.CurrentCulture);
            }
        }

        public bool OccursOnGivenDay(DateTime dateTime)
        {
            if (DueDate == dateTime)
            {
                return true;
            }

            var convertedDateTime = new LocalDate(dateTime.Year, dateTime.Month, dateTime.Day);
            LocalDate localDate = new LocalDate(DueDate.Year, DueDate.Month, DueDate.Day);

            for (int i = 0; i < Frequency.NumberOfOccurrencesPerYear; i++)
            {
                localDate = localDate.Plus(Frequency.TotalPeriod);
                if (localDate == convertedDateTime)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
