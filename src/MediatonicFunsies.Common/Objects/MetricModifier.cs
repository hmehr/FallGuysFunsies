using System;

namespace MediatonicFunsies.Common.Objects
{
    public class MetricModifier
    {
        
        public DateTime ActionDate { get; set; }

        /// <summary>
        /// The percentage of metric change. E.g. +0.8 for stroking yields 80% of happiness boost
        /// </summary>
        public double ModificationEffect { get; set; }
    }
}