using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyCloakAuthenticatedAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {

        //View Sale
        [HttpGet]
        [Authorize(Policy = "view_sale")]
        public IActionResult GetSale()
        {
            return Ok(new[] { new { Id = 1, Name = "Sale1" }, new { Id = 2, Name = "Sale2" } });
        }
        //Edit Sale
        [HttpPost]
        [Authorize(Policy = "edit_sale")]
        public IActionResult EditSale()
        {
            return Ok(new { Id = 1, Name = "Sale1" });
        }
    }
}
