using Microsoft.AspNetCore.Mvc;
using MyWallet.DTOs;
using MyWallet.Middlewares;
using MyWallet.Services;

namespace MyWallet.Controllers;

[ApiController]
[Route("/keys")]

public class KeysController : ControllerBase
{

    private readonly KeysService _keysService;
    private readonly AuthorizationMiddleware _authorizationMiddleware;

    public KeysController(KeysService keysService, AuthorizationMiddleware authorizationMiddleware)
    {
        _keysService = keysService;
        _authorizationMiddleware = authorizationMiddleware;
    }

    [HttpPost]
    public async Task<IActionResult> CreateKey(CreateKeyDTO dto)
    {
        string? token = Request.Headers.Authorization;
        if (token == null) return Unauthorized("Payment Provider Token is missing");
        int id = await _authorizationMiddleware.ValidatePSPToken(token);

        await _keysService.CreateKey(dto.ToEntity(), id);
        return CreatedAtAction(null, null, dto.ToEntity());
    }
}
