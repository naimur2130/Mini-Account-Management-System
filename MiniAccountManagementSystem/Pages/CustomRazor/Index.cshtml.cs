using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAccountManagementSystem.Pages.CustomRazor
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _user;
        public IndexModel(UserManager<IdentityUser> user)
        {
            _user = user;
        }

        public List<IdentityUser> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = new List<IdentityUser>(_user.Users);
        }
    }
}
