using Microsoft.AspNetCore.Mvc;
using MyWallet.DTOs;
using MyWallet.Entities;
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
    private readonly GetKeyByValueDTO _getKeyByValueDTO;

    public KeysController(KeysService keysService, AuthorizationMiddleware authorizationMiddleware, GetKeyByValueDTO getKeyByValueDTO)
    {
        _keysService = keysService;
        _authorizationMiddleware = authorizationMiddleware;
        _getKeyByValueDTO = getKeyByValueDTO;
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

    [HttpGet("{type}/{value}")]
    public async Task<IActionResult> GetKeyByValue(string type, string value)
    {

        string? token = Request.Headers.Authorization;
        if (token == null) throw new UnauthorizedError("Payment Provider Token is missing");
        int id = await _authorizationMiddleware.ValidatePSPToken(token);

        PixKeys? key = await _keysService.GetKeyByValue(type, value);
        Console.WriteLine(key.Account?.Agency);
        var responseBody = _getKeyByValueDTO.ConvertDataToGetKeyByValue(key);

        return Ok(responseBody);
    }
}
