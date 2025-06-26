using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MiniAccountManagementSystem.Models;
using System.Data;

namespace MiniAccountManagementSystem.Pages.Vouchers
{
    [Authorize(Roles = "Accountant")]
    public class VoucherEntryPageModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        public VoucherEntryPageModel(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        [BindProperty]
        public VoucherModel Voucher { get; set; }
        public List<AccountDropdownItem> Accounts { get; set; }

        
        public void OnGet()
        {
            Voucher = new VoucherModel
            {
                VoucherDate = DateTime.Today,
                VoucherEntries = new List<VoucherEntryModel>
                {
                    new VoucherEntryModel ()
                }
            };
            LoadAccounts();
        }

        public IActionResult OnPost()
        {
            LoadAccounts(); 
            if (Voucher == null || Voucher.VoucherEntries == null || Voucher.VoucherEntries.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Please add at least one voucher entry.");
                return Page();
            }

            decimal totalDebit = Voucher.VoucherEntries.Sum(e => e.DebitAmount);
            decimal totalCredit = Voucher.VoucherEntries.Sum(e => e.CreditAmount);

            if (totalDebit != totalCredit)
            {
                ModelState.AddModelError(string.Empty, "Total Debit must equal Total Credit.");
                return Page();
            }

            try
            {
                var dt = new DataTable();
                dt.Columns.Add("AccountId", typeof(int));
                dt.Columns.Add("DebitAmount", typeof(decimal));
                dt.Columns.Add("CreditAmount", typeof(decimal));
                dt.Columns.Add("Narration", typeof(string));

                foreach (var entry in Voucher.VoucherEntries)
                {
                    dt.Rows.Add(entry.AccountId, entry.DebitAmount, entry.CreditAmount, entry.Narration);
                }

                using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (var cmd = new SqlCommand("sp_SaveVoucher", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@VoucherType", Voucher.VoucherType);
                        cmd.Parameters.AddWithValue("@VoucherDate", Voucher.VoucherDate);
                        cmd.Parameters.AddWithValue("@ReferenceNo", Voucher.ReferenceNo);

                        string createdBy = _userManager.GetUserId(User); 
                        cmd.Parameters.AddWithValue("@CreatedBy", createdBy);

                        var tvpParam = cmd.Parameters.AddWithValue("@VoucherEntries", dt);
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        tvpParam.TypeName = "VoucherEntryType";

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["SuccessMessage"] = "Voucher saved successfully!";
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error saving voucher: " + ex.Message);
                return Page();
            }
        }


        private void LoadAccounts()
        {
            Accounts = new List<AccountDropdownItem>();

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("sp_GetChartOfAccounts", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Accounts.Add(new AccountDropdownItem
                    {
                        AccountId = reader.GetInt32(0),
                        AccountName = reader.GetString(1)
                    });
                }
            }
        }



    }
    public class AccountDropdownItem
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
    }
}
