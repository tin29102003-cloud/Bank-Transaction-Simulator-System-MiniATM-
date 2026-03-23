using System;
using System.Collections.Generic;
using System.Text;

namespace MiniATM.Entities.DomainEvents
{
    internal class DomainChangedEvent:DomainEvent
    {
        public required string Account {  get; set; }
        public required double NewBalance { get; set; }
        public required double OldBalance { get; set; }
    }
}
