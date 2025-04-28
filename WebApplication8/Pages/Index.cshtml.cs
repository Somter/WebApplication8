using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebApplication8.Models;
using WebApplication8.Repositories;

namespace WebApplication8.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IRepository<Message> _messageRepo;
        private readonly IUserRepository _userRepo;
        private readonly IHttpContextAccessor _accessor;

        public IndexModel(IRepository<Message> messageRepo, IUserRepository userRepo, IHttpContextAccessor accessor)
        {
            _messageRepo = messageRepo;
            _userRepo = userRepo;
            _accessor = accessor;
        }

        [BindProperty]
        [Required, StringLength(500)]
        public string MessageText { get; set; }

        public List<Message> Messages { get; set; } = new();

        public Dictionary<int, string> UserNamesById { get; set; } = new();

        public async Task OnGetAsync()
        {
            Messages = (await _messageRepo.GetAllAsync()).OrderByDescending(m => m.MessageDate).ToList();
            var users = await _userRepo.GetAllAsync();

            UserNamesById = users.ToDictionary(u => u.Id, u => u.Name);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var userId = int.Parse(_accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var msg = new Message
            {
                Id_User = userId,
                MessageText = MessageText
            };

            await _messageRepo.AddAsync(msg);
            await _messageRepo.SaveAsync();

            return RedirectToPage();
        }
    }
}
