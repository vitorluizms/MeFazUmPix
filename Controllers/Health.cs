using Microsoft.AspNetCore.Mvc;

using MyWallet.Services;

namespace MyWallet.Controllers;

[ApiController]
[Route("[controller]")]

public class HealthController : ControllerBase
{
    private readonly HealthService _healthService;

    public HealthController(HealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        string message = _healthService.GetHealth();
        return Ok(message);
    }
}
