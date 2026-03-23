namespace MiniATM.Entities.DomainEvents
{
    internal class DomainEvent
    {
        public required DateOnly EventTimeUtc { get; set; }
    }
}