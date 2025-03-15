using HubService.Infrastructure;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace HubService.API.Controllers
{
    public class ODataBaseController : ODataController
    {
        private ApplicationDbContext _dataContext;

        protected ApplicationDbContext DataContext => _dataContext ??= HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

    }
}
