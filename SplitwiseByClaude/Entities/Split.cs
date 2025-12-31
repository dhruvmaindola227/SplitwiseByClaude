namespace SplitwiseByClaude.Entities
{
    public class Split
    {
        public string UserEmail { get; set; } = default!;
        public int Amount { get; set; } = default!;
        public int? Percentage { get; set; } = default!;
    }
}