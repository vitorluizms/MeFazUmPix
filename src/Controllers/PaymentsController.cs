using Microsoft.AspNetCore.Mvc;
using MyWallet.DTOs;
using MyWallet.Entities;
using MyWallet.Exceptions;
using MyWallet.Middlewares;
using MyWallet.Models;
using MyWallet.Services;

namespace MyWallet.Controllers;

[ApiController]
[Route("/payments")]

public class PaymentsController : ControllerBase
{
    private readonly AuthorizationMiddleware _authorizationMiddleware;

    public PaymentsController(AuthorizationMiddleware authorizationMiddleware)
    {
        _authorizationMiddleware = authorizationMiddleware;
    }

    public async Task<IActionResult> CreatePaymentDTO(CreatePaymentDTO dto)
    {
        string? token = Request.Headers.Authorization;
        if (token == null) throw new UnauthorizedError("Payment Provider Token is missing");
        PaymentProvider paymentProvider = await _authorizationMiddleware.ValidatePSPToken(token);

        return CreatedAtAction(null, null, 'a');
    }
}