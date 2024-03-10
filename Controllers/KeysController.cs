using Microsoft.AspNetCore.Mvc;
using MyWallet.DTOs;
using MyWallet.Exceptions;
using MyWallet.Middlewares;
using MyWallet.Models;
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

    private RequestBody ConvertDtoToRequestBody(CreateKeyDTO dto)
    {
        var requestBody = new RequestBody
        {
            Key = new KeyDTO
            {
                Value = dto.ToEntity().Value,
                Type = dto.ToEntity().Type
            },
            User = new UserDTO
            {
                CPF = dto.ToEntity().CPF
            },
            Account = new AccountDTO
            {
                Number = dto.ToEntity().Number,
                Agency = dto.ToEntity().Agency
            }
        };

        return requestBody;
    }


    [HttpPost]
    public async Task<IActionResult> CreateKey(CreateKeyDTO dto)
    {
        string? token = Request.Headers.Authorization;
        if (token == null) throw new UnauthorizedError("Payment Provider Token is missing");
        int id = await _authorizationMiddleware.ValidatePSPToken(token);

        await _keysService.CreateKey(dto.ToEntity(), id);
        var requestBody = ConvertDtoToRequestBody(dto);
        return CreatedAtAction(null, null, requestBody);
    }
}
