namespace NaniTrader.Entities
{
    public class UnderlyingSecurity
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public Guid ParentId { get; set; }
    }
}