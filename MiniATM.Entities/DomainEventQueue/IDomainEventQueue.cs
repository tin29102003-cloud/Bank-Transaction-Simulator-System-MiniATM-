using MiniATM.Entities.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniATM.Entities.DomainEventQueue
{
    internal interface IDomainEventQueue<T> where T: DomainEvent//bắt buộc thang T truyền vào phải kế thừa domainEvent
    {
        void Enqueue(T evt);
    }
}
