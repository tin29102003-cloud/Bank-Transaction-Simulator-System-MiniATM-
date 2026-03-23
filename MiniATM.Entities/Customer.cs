using System;
using System.Collections.Generic;
using System.Text;

namespace MiniATM.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
