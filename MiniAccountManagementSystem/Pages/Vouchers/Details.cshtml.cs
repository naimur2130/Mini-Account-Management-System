using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MiniAccountManagementSystem.Pages.Vouchers
{
    public class DetailsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public DetailsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int VoucherId { get; set; }
        public List<VoucherEntryDetail> Entries { get; set; }

        public IActionResult OnGet(int id)
        {
            VoucherId = id;
            Entries = new List<VoucherEntryDetail>();

            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var cmd = new SqlCommand("sp_GetVoucherEntriesByVoucherId", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@VoucherId", id);

                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Entries.Add(new VoucherEntryDetail
                    {
                        EntryId = reader.GetInt32(0),
                        AccountId = reader.GetInt32(1),
                        AccountName = reader.GetString(2),
                        DebitAmount = reader.GetDecimal(3),
                        CreditAmount = reader.GetDecimal(4),
                        Narration = reader.GetString(5)
                    });
                }
            }

            return Page();
        }

        public class VoucherEntryDetail
        {
            public int EntryId { get; set; }
            public int AccountId { get; set; }
            public string AccountName { get; set; }
            public decimal DebitAmount { get; set; }
            public decimal CreditAmount { get; set; }
            public string Narration { get; set; }
        }
    }
}
