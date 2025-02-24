using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyCloakAuthenticatedAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetEmployees()
        {
            return Ok(new[] { new { Id = 1, Name = "John Doe" }, new { Id = 2, Name = "Jane Doe" } });
        }
    }
}
