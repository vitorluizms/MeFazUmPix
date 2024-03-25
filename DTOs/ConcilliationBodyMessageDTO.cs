using System.ComponentModel.DataAnnotations;
using MyWallet.DTOs;

namespace MyWallet.DTOs;
public class ConcilliationBodyMessageDTO(int paymentProviderId, ConcilliationDTO dto)
{
  public int PaymentProviderId { get; } = paymentProviderId;
  public string Date { get; } = dto.Date.ToString("yyyy-MM-dd");
  public string File { get; } = dto.File;
  public string Postback { get; } = dto.Postback;
}