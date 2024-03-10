using Microsoft.AspNetCore.Mvc;
using MyWallet.DTOs;

namespace MyWallet.Controllers;

[ApiController]
[Route("/keys")]

public class KeysController : ControllerBase
{

    [HttpPost]
    public IActionResult CreateKey(CreateKeyDTO dto)
    {
        return CreatedAtAction(null, null, dto.ToEntity());
    }
}
