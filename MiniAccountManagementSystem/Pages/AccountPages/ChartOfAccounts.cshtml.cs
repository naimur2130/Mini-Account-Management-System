using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using MiniAccountManagementSystem.Utilities;

namespace MiniAccountManagementSystem.Pages.AccountPages
{
    [Authorize(Roles = "Admin,Accountant")]
    public class ChartOfAccountsModel : PageModel
    {
        private readonly IConfiguration _config;
        public ChartOfAccountsModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty]
        public string AccountName { get; set; }

        [BindProperty]
        public int? ParentAccountId { get; set; }

        [BindProperty]
        public string AccountType { get; set; }

        public List<AccountItem> ExistingAccounts { get; set; } = new List<AccountItem>();
        public List<AccountItem> AllAccounts { get; set; } = new List<AccountItem>();


        public class AccountItem
        {
            public int AccountId { get; set; }
            public string AccountName { get; set; }
            public int? ParentAccountId { get; set; }
            public string AccountType { get; set; }
        }

        // Load all accounts for display & also load ExistingAccounts for dropdown
        public async Task OnGetAsync()
        {
            await LoadAccountsAsync();

            ExistingAccounts = AllAccounts.Select(a => new AccountItem
            {
                AccountId = a.AccountId,
                AccountName = a.AccountName
            }).ToList();
        }

        private async Task LoadAccountsAsync()
        {
            AllAccounts = new List<AccountItem>();

            using (var con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            using (var command = new SqlCommand("SELECT AccountId, AccountName, ParentAccountId, AccountType FROM ChartOfAccounts WHERE IsActive=1", con))
            {
                await con.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    AllAccounts.Add(new AccountItem
                    {
                        AccountId = reader.GetInt32(0),
                        AccountName = reader.GetString(1),
                        ParentAccountId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                        AccountType = reader.GetString(3)
                    });
                }
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            using (var con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            using (var command = new SqlCommand("sp_ManageChartOfAccounts", con))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "CREATE");
                command.Parameters.AddWithValue("@AccountName", AccountName);
                command.Parameters.AddWithValue("@ParentAccountId", ParentAccountId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@AccountType", AccountType);

                await con.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToPage("ChartOfAccounts");
        }
    }
}
