using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;


namespace MediatonicFunsies.Common.Objects
{
    public class Metric
    {
        /// <summary>
        /// The linear constant rate at which the metric increases or decreases. For decreasing metrics, we use negative rates.
        /// For example a rate of 1.0 increases happiness level by one each second over time.
        /// </summary>
        public double Rate { get; set; }

        public Guid Id { get; set; }

        public double Value
        {
            get
            {
                double x = (DateTime.Now - CreationDate).TotalSeconds * Rate + InitialStateValue + Modifiers?.Sum(m =>
                {
                    double metricValue = (m.ActionDate - CreationDate).TotalSeconds * Rate + InitialStateValue;
                    double modificationValue = metricValue * m.ModificationEffect;
                    return modificationValue;
                }) ?? 0;

                return Math.Round(x, 2, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// Get the value of the metric at a certain moment during the life of the animal
        /// </summary>
        public double ValueAtDateTime(DateTime dateTime)
        {
            if (dateTime < CreationDate)
            {
                return InitialStateValue;
            }

            double x = (dateTime - CreationDate).TotalSeconds * Rate + 
                   InitialStateValue + Modifiers?.Where(m => m.ActionDate <= dateTime)
                       .Sum(m =>
                {
                    double metricValue = (m.ActionDate - CreationDate).TotalSeconds * Rate + InitialStateValue;
                    double modificationValue = metricValue * m.ModificationEffect;
                    return modificationValue;
                }) ?? 0;
            return Math.Round(x, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Hunger, Happiness, Fatigue, etc.
        /// </summary>
        public MetricTypes Type { get; set; }

        public IEnumerable<MetricModifier> Modifiers { get; set; }

        /// <summary>
        /// The initial value of the metric for which the animals are born with in the world.
        /// For example the happiness level is at 100, and hunger at 0
        /// </summary>
        public double InitialStateValue { get; set; }

        public DateTime CreationDate { get; set; }
    }
}