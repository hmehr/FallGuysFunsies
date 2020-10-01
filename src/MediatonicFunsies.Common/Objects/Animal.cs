using System;
using System.Collections.Generic;

namespace MediatonicFunsies.Common.Objects
{
    public class Animal
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public IEnumerable<Metric> Metrics { get; set; }

        public Guid Owner { get; set; }

        public DateTime CreationDate { get; set; }
    }
}