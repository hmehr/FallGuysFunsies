using System;
using System.Collections.Generic;
using System.Linq;
using MediatonicFunsies.Common.Objects;
using Xunit;

namespace MediatonicFunsies.UnitTest
{
    public class MetricsTests
    {
        [Fact]
        public void Should_Get_The_Right_Metric_Value_For_A_Positive_Rate_And_Initial_State_Of_Zero()
        {
            var animal = new Animal();
            DateTime animalCreationDate = DateTime.Now - TimeSpan.FromSeconds(10);
            animal.CreationDate = animalCreationDate;

            animal.Metrics = new List<Metric>
            {
                new Metric
                {
                    CreationDate = animalCreationDate,
                    InitialStateValue = 0,
                    Modifiers = new List<MetricModifier>
                    {
                        new MetricModifier
                            {
                                ActionDate = DateTime.Now - TimeSpan.FromSeconds(5), 
                                ModificationEffect = -0.1
                            }
                    },
                    Rate = 1
                }
            };

            Assert.Equal(10 + ( -0.1 * 5), animal.Metrics.First().Value);

        }

        [Fact]
        public void Should_Get_The_Right_Metric_Value_For_A_Negative_Rate_And_Initial_State_Of_10()
        {
            var animal = new Animal();
            DateTime animalCreationDate = DateTime.Now - TimeSpan.FromSeconds(10);
            animal.CreationDate = animalCreationDate;

            animal.Metrics = new List<Metric>
            {
                new Metric
                {
                    CreationDate = animalCreationDate,
                    InitialStateValue = 10,
                    Modifiers = new List<MetricModifier>
                    {
                        new MetricModifier
                        {
                            ActionDate = DateTime.Now - TimeSpan.FromSeconds(5), 
                            ModificationEffect = 0.1
                        }
                    },
                    Rate = -1
                }
            };

            Assert.Equal(-10 +10 + ( 0.1 * 5), animal.Metrics.First().Value);

        }
    }
}
