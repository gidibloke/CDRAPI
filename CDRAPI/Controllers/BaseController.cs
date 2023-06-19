using CDRAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CDRAPI.Controllers
{
    //
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private ApplicationDbContext _context;
        protected ApplicationDbContext Context => _context ??= HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

    }

}
