namespace MiniAccountManagementSystem.Models
{
    public class VoucherEntryModel
    {
        public int AccountId { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Narration { get; set; }
    }
}
