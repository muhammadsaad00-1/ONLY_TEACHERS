using OT.Authorization;
using OT.Data;
using OT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OT.Areas.Identity.Data;
using System.Net;
using System.Text.RegularExpressions;

namespace OT.Pages.Contacts
{
    #region snippet

    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(
            OTContext context,
            IAuthorizationService authorizationService,
            UserManager<OTUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IList<Post> Post { get; set; }
         public string GetPlainText(string content)
        {
            if(content == null){
                return string.Empty;
            }
            if (HasHtmlTags(content))
            {
                // Decode HTML entities
                string decodedHtml = WebUtility.HtmlDecode(content);
                // Remove HTML tags using Regex
                string plainText = Regex.Replace(decodedHtml, "<[^>]*>", "");
                // Trim whitespace
                plainText = plainText.Trim();
                return plainText;
            }
            else
            {
                // Return the content as is if it doesn't contain HTML tags
                return content;
            }
        }

        private bool HasHtmlTags(string content)
        {
            // Check if the content contains any HTML tags
            return Regex.IsMatch(content, @"<[^>]+>");
        }

        public async Task OnGetAsync()
        {
         
            var contacts = from c in Context.Post
                           select c;

            var isAuthorized = User.IsInRole("Manager") ||
                               User.IsInRole("Admin");

            var currentUserId = UserManager.GetUserId(User);

            // Only approved contacts are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                contacts = contacts.Where(c => c.Status == ContactStatus.Approved
                                            || c.OwnerID == currentUserId);
            }

            Post = await contacts.ToListAsync();
        }
    }
    #endregion
}