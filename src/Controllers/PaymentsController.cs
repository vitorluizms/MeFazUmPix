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
    private readonly PaymentsService _paymentsService;

    public PaymentsController(AuthorizationMiddleware authorizationMiddleware, PaymentsService paymentsService)
    {
        _authorizationMiddleware = authorizationMiddleware;
        _paymentsService = paymentsService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaymentDTO(CreatePaymentDTO dto)
    {
        string? token = Request.Headers.Authorization;
        if (token == null) throw new UnauthorizedError("Payment Provider Token is missing");
        PaymentProvider paymentProvider = await _authorizationMiddleware.ValidatePSPToken(token);

        int paymentId = await _paymentsService.CreatePayment(dto, paymentProvider);


        return CreatedAtAction(null, null, paymentId);
    }
}