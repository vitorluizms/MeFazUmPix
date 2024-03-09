using Microsoft.AspNetCore.Mvc;

namespace MyWallet.Controllers;

[ApiController]
[Route("[controller]")]

public class KeysController : ControllerBase
{

    [HttpGet]
    public IActionResult CreateKey()
    {
        return Ok("Get keys");
    }
}
