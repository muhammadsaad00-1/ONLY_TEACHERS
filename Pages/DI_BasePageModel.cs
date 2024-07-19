using OT.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OT.Areas.Identity.Data;

namespace OT.Pages.Contacts
{
    [Authorize]
    public class DI_BasePageModel : PageModel
    {
        protected OTContext Context { get; }
        public IAuthorizationService AuthorizationService { get; }
        protected UserManager<OTUser> UserManager { get; }

        public DI_BasePageModel(
            OTContext context,
            IAuthorizationService authorizationService,
            UserManager<OTUser> userManager) : base()
        {
            Context = context;
            UserManager = userManager;
            AuthorizationService = authorizationService;
        } 
    }
}
