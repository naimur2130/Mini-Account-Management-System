using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MiniAccountManagementSystem.Pages.Vouchers
{
    [Authorize(Roles = "Admin,Accountant,Viewer")]
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<VoucherListItem> Vouchers { get; set; }

        public void OnGet()
        {
            Vouchers = new List<VoucherListItem>();

            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var cmd = new SqlCommand("sp_GetVouchers", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Vouchers.Add(new VoucherListItem
                    {
                        VoucherId = reader.GetInt32(0),
                        VoucherType = reader.GetString(1),
                        VoucherDate = reader.GetDateTime(2),
                        ReferenceNo = reader.GetString(3),
                        CreatedOn = reader.GetDateTime(4)
                    });
                }
            }
        }

        public class VoucherListItem
        {
            public int VoucherId { get; set; }
            public string VoucherType { get; set; }
            public DateTime VoucherDate { get; set; }
            public string ReferenceNo { get; set; }
            public DateTime CreatedOn { get; set; }
        }
    }
}
