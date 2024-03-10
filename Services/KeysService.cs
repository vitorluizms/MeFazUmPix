using MyWallet.Entities;
using MyWallet.Exceptions;
using MyWallet.Models;
using MyWallet.Repositories;

namespace MyWallet.Services;

public class KeysService
{

    private readonly KeyRepository _keyRepository;
    private readonly UserRepository _userRepository;
    private readonly AccountRepository _accountRepository;

    public KeysService(KeyRepository keyRepository, UserRepository userRepository, AccountRepository accountRepository)
    {
        _keyRepository = keyRepository;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
    }

    public async Task CreateKey(PixKeyCreationData dto, int id)
    {
        if (dto.Type == "CPF" && dto.Value != dto.CPF) throw new BadRequestError("CPF and Value must be the same");

        Users user = await ValidateUser(dto.CPF);
        List<Account> accounts = await _accountRepository.GetAccountsByCPF(user.Id);
        if (!accounts.Any(a => a.Number == dto.Number && a.Agency == dto.Agency))
        {
            Account newAccount = await ValidateAccount(user.Id, dto.Number, dto.Agency, id);
#pragma warning disable IDE0305
            accounts = accounts.Append(newAccount).ToList();

        }
    }

    private async Task<Users> ValidateUser(string cpf)
    {
        Users? user = await _userRepository.GetUserByCPF(cpf) ?? throw new NotFoundError("User not found");

        return user;
    }

    private async Task<Account> ValidateAccount(int id, int number, int agency, int paymentProviderId)
    {
        // Create a new Account if user doesn't have any account or if the account doesn't exist
        return await _accountRepository.CreateAccount(id, number, agency, paymentProviderId);
    }

}
