// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.IO;
using System.Threading.Tasks;
using Chirp.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ProfilePictureModel : PageModel
    {
        private readonly UserManager<Author> _userManager;
        private readonly IWebHostEnvironment _environment;

        public ProfilePictureModel(
            UserManager<Author> userManager,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _environment = environment;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public string CurrentPicturePath { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public IFormFile Picture { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Index");
            }

            CurrentPicturePath = user.ProfilePicturePath;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || Input.Picture == null)
            {
                return Page();
            }

            var uploads = Path.Combine(_environment.WebRootPath, "profile-pics");
            Directory.CreateDirectory(uploads);

            var fileName = $"{user.Id}{Path.GetExtension(Input.Picture.FileName)}";
            var filePath = Path.Combine(uploads, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await Input.Picture.CopyToAsync(stream);

            user.ProfilePicturePath = $"/profile-pics/{fileName}";
            await _userManager.UpdateAsync(user);

            StatusMessage = "Your profile picture has been updated.";
            return RedirectToPage();
        }
    }
}