using MyWallet.DTOs;
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

    public async Task<PixKeys> CreateKey(PixKeyCreationData dto, int id)
    {
        if (dto.Type == "CPF" && dto.Value != dto.CPF) throw new BadRequestError("CPF and Value must be the same");

        Users user = await ValidateUser(dto.CPF);
        Account? account = await _accountRepository.GetAccountByNumber(dto.Number) ?? await CreateNewAccount(user.Id, dto.Number, dto.Agency, id);

        await ValidateKeysByUser(dto.Type, user.Id, dto.Value, account.Id);
        PixKeys key = await _keyRepository.CreateKey(dto.Type, dto.Value, account.Id, id);

        return key;
    }

    public async Task<Users> ValidateUser(string cpf)
    {
        Users? user = await _userRepository.GetUserByCPF(cpf) ?? throw new NotFoundError("User not found");

        return user;
    }

    private async Task<Account> CreateNewAccount(int id, int number, int agency, int paymentProviderId)
    {
        // Create a new Account if user doesn't have any account or if the account doesn't exist
        return await _accountRepository.CreateAccount(id, number, agency, paymentProviderId);
    }

    private async Task ValidateKeysByUser(string type, int id, string value, int accountId)
    {
        List<PixKeys> keys = await _keyRepository.GetPixKeyByUserId(id);

        if (keys.Count == 20) throw new ConflictError("User already has 20 keys");

        else if (type == "CPF" && keys.Any(k => k.Type == "CPF")) throw new ConflictError("User already has a CPF key");

        else if (keys.Any(k => k.Value == value)) throw new ConflictError("Key already exists");

        IEnumerable<IGrouping<int, PixKeys>> accountWith5Keys = keys.GroupBy(k => k.AccountId).Where(grupo => grupo.Key == accountId && grupo.Count() == 5);
        if (accountWith5Keys.Any()) throw new ConflictError("This account have 5 keys already");
    }

    public async Task<PixKeys> GetKeyByValue(string type, string value)
    {
        PixKeys? key = await _keyRepository.GetPixKeyByValue(type, value) ?? throw new NotFoundError("Key not found");
        return key;
    }

}
