using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyCloakAuthenticatedAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POController : ControllerBase
    {

        //View PO
        [HttpGet("GetPO")]
        [Authorize(Roles = "view_po")]
        public IActionResult GetPO()
        {
            //var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            //return Ok(claims);
            return Ok(new[] { new { Id = 1, Name = "PO1" }, new { Id = 2, Name = "PO2" } });
        }

        //Edit PO
        [HttpPost("EditPO")]
        [Authorize(Roles = "edit_po")]
        public IActionResult EditPO()
        {
            return Ok(new { Id = 1, Name = "PO1" });
        }
    }
}
