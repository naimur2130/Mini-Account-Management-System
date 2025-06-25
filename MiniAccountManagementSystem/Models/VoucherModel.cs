namespace MiniAccountManagementSystem.Models
{
    public class VoucherModel
    {
        public string VoucherType { get; set; }
        public DateTime VoucherDate { get; set; }
        public string ReferenceNo { get; set; }
        public List<VoucherEntryModel> VoucherEntries { get; set; }
    }
}
