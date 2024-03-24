using System.ComponentModel.DataAnnotations;

namespace MyWallet.DTOs;

public class ConcilliationDTO(DateTime date, string file, string postback)
{
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Field date is required")]
    public DateTime Date { get; } = date;

    // TODO: test with Uri type
    [Required(ErrorMessage = "Field file is required")]
    public string File { get; } = file;

    [Required(ErrorMessage = "Field postback is required")]
    public string Postback { get; } = postback;
}