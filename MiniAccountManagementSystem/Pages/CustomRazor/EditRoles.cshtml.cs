using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniAccountManagementSystem.Utilities;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace MiniAccountManagementSystem.Pages.CustomRazor
{
    [Authorize(Roles = "Admin")]
    public class EditRolesModel : PageModel
    {
        private readonly UserManager<IdentityUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public EditRolesModel(UserManager<IdentityUser> user, 
            RoleManager<IdentityRole> role)
        {
            _user = user;
            _role = role;
        }

        public IdentityUser User { get; set; }
        public List<string> Roles { get; set; }
        public List<String> UserRoles { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            User = await _user.FindByIdAsync(id);
            if(User == null) return NotFound();
            Roles = _role.Roles.Select(x => x.Name).ToList();
            UserRoles = (await _user.GetRolesAsync(User)).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id, 
            List<string>SelectedRoles)
        {
            var user = await _user.FindByIdAsync(id);
            var currentRoles = await _user.GetRolesAsync(user);

            await _user.RemoveFromRolesAsync(user,currentRoles);
            await _user.AddToRolesAsync(user,SelectedRoles);
            return RedirectToPage("Index");
        }
    }
}
