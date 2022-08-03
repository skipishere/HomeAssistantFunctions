using System;
using System.Collections.Generic;

namespace BinCollection
{
    public record Covid
    {
        public IEnumerable<Data> Data { get; set; }
    }

    public record Data
    {
        public DateTime Date { get; set; }

        public int? New { get; set; }

        public int? Total { get; set; }

        public float? FirstDosePercentage { get; set; }

        public float? SecondDosePercentage { get; set; }

        public float? ThirdDosePercentage { get; set; }
    }
}
