namespace GlobalBlueProject.Models
{
    public sealed class PurchaseAmountsDTO
    {
        public int VatRate { get; set; }
        public decimal NetValue { get; set; }
        public decimal GrossValue { get; set; }
        public decimal VatValue { get; set; }
    }
}
