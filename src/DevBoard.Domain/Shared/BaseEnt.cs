namespace DevBoard.Domain.Shared
{
    public class BaseEnt
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
    }
}
