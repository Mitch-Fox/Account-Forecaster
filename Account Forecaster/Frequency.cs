using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Account_Forecaster
{
    public sealed class Frequency
    {
        public static readonly List<Frequency> AllFrequencies = new List<Frequency>();

        public static readonly Frequency Weekly = new Frequency(52, "Weekly", Period.FromWeeks(1));
        public static readonly Frequency Biweekly = new Frequency(26, "Bi-weekly", Period.FromWeeks(2));
        public static readonly Frequency Monthly = new Frequency(12, "Monthly", Period.FromMonths(1));
        public static readonly Frequency Quarterly = new Frequency(4, "Quarterly", Period.FromMonths(3));
        public static readonly Frequency Yearly = new Frequency(1, "Yearly", Period.FromYears(1));

        public readonly int NumberOfOccurrencesPerYear;
        public readonly string DisplayName;
        public readonly Period TotalPeriod;

        private Frequency() { }

        private Frequency(int numberOfOccurrencesPerYear, string displayName, Period period)
        {
            NumberOfOccurrencesPerYear = numberOfOccurrencesPerYear;
            DisplayName = displayName;
            TotalPeriod = period;

            AllFrequencies.Add(this);
        }

        public override string ToString() => DisplayName;

        public static Frequency GetFrequencyFromString(string displayName) => AllFrequencies.FirstOrDefault(x => x.DisplayName == displayName);
    }
}
