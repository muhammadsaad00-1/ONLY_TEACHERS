using OT.Authorization;
using OT.Data;
using OT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using OT.Areas.Identity.Data;
using ContactManager.Authorization;

namespace OT.Pages.Contacts
{
    #region snippetCtor
    [Authorize]

    public class CreateModel : DI_BasePageModel
    {
        public CreateModel(
            OTContext context,
            IAuthorizationService authorizationService,
            UserManager<OTUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }
        #endregion

        public async Task<IActionResult> OnGet() 
        {
            var currentUserID = UserManager.GetUserId(User);
            var userObj = await UserManager.FindByIdAsync(currentUserID);
            if(userObj.UserType == "Student"){
                return Forbid();
            }
            Post = new Post
            {
                Title = "This is My first post",
                PostDate = DateTime.Now,
                content = "Horny teachers nearby",
                Price = 7.99M,
                subscribers = 0
                
            };
            return Page();
        }

        [BindProperty]
        public Post Post { get; set; }

        #region snippet_Create
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Post.OwnerID = UserManager.GetUserId(User);

            // requires using ContactManager.Authorization;
            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User, Post,
                                                        ContactOperations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            Post.content = Request.Form["RichTextEditor"];

            Context.Post.Add(Post);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        #endregion
    }
}