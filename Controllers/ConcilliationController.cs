
using Microsoft.AspNetCore.Mvc;
using MyWallet.DTOs;
using MyWallet.Entities;
using MyWallet.Exceptions;
using MyWallet.Middlewares;
using MyWallet.Models;
using MyWallet.Services;

namespace MyWallet.Controllers;

[ApiController]
[Route("/concilliation")]

public class ConcilliationController(AuthorizationMiddleware authorizationMiddleware, ConcilliationService concilliationService) : ControllerBase
{
    private readonly AuthorizationMiddleware _authorizationMiddleware = authorizationMiddleware;
    private readonly ConcilliationService _concilliationService = concilliationService;
    [HttpPost]
    public async Task<IActionResult> PostConcilliation(ConcilliationDTO dto)
    {
        string? token = Request.Headers.Authorization;
        if (token == null) throw new UnauthorizedError("Payment Provider Token is missing");
        PaymentProvider paymentProvider = await _authorizationMiddleware.ValidatePSPToken(token);

        _concilliationService.PostConcilliationService(dto, paymentProvider.Id);

        return Ok();
    }
}