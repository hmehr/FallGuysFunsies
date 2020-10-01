using System;
using System.Collections.Generic;

namespace MediatonicFunsies.Common.Objects
{
    public class Player
    {
        public string Name { get; set; }

        public IEnumerable<Animal> Animals { get; set; }

        public Guid Id { get; set; }
    }
}