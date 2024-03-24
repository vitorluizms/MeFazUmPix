using Microsoft.AspNetCore.Http.HttpResults;
using MyWallet.DTOs;
using MyWallet.Exceptions;

public class ConcilliationService(MessageService messageService)
{
    private readonly MessageService _messageService = messageService;
    public void PostConcilliationService(ConcilliationDTO dto, int pspId)
    {
        if (dto.Date > DateTime.Today) throw new BadRequestError("Date cannot be in the future");
        ConcilliationBodyMessageDTO message = new(pspId, dto);

        _messageService.SendPaymentMessage(message, "concilliation");
    }
}