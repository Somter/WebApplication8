using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WebApplication8.Models;
using WebApplication8.Repositories;
using WebApplication8.Services;

namespace WebApplication8.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _hasher;

        public RegisterModel(IUserRepository userRepository, IPasswordHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        [BindProperty]
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [BindProperty, Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [BindProperty, Required, Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            if (await _userRepository.GetByNameAsync(Name) != null)
            {
                ErrorMessage = "Користувач уже існує.";
                return Page();
            }

            var salt = _hasher.GenerateSalt();
            var hashedPwd = _hasher.HashPassword(Password, salt);

            var user = new User
            {
                Name = Name,
                Pwd = hashedPwd,
                Salt = salt
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();

            return RedirectToPage("Login");
        }
    }
}
