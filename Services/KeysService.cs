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

        if (await _keyRepository.GetOnlyPixKeyByValue(dto.Value) is not null)
        {
            Console.WriteLine("Key already exists");
            throw new ConflictError("Key already exists");
        }

        Users user = await ValidateUser(dto.CPF);
        if (user.Accounts is not null) // Check if user.Accounts is not null
        {
            ValidateKeysByUser(user.Accounts, dto.Value);
        }

        Account? account = user.Accounts?.FirstOrDefault(a => a.Agency == dto.Agency && a.Number == dto.Number);

        if (account is not null) ValidatePixKeysByAccount(account.PixKeys);
        else
        {
            await ValidateAccount(dto.Number, dto.Agency);
            account = await _accountRepository.CreateAccount(user.Id, dto.Number, dto.Agency, id);
        }
        PixKeys key = await _keyRepository.CreateKey(dto.Type, dto.Value, account.Id, id);
        return key;

    }

    public async Task<Users> ValidateUser(string cpf)
    {
        Users? user = await _userRepository.GetUserByCpfIncludeAccountsThenIncludePixKeys(cpf) ?? throw new NotFoundError("User not found");

        return user;
    }

    public async Task ValidateAccount(int number, int agency)
    {
        if (await _accountRepository.GetAccountByNumberAndAgency(number, agency) is not null)
        {
            throw new ConflictError("This account pertains to another user");
        }
    }


    private static void ValidatePixKeysByAccount(ICollection<PixKeys>? pixKeys)
    {
        if (pixKeys is not null && pixKeys.Count >= 5)
        {
            throw new ConflictError("This account have 5 keys already");
        }
    }

    private static void ValidateKeysByUser(ICollection<Account> accounts, string value)
    {
        int totalKeysCount = 0;
        bool hasCpfKey = false;

        foreach (var account in accounts)
        {
            totalKeysCount += account.PixKeys?.Count ?? 0;
            hasCpfKey = account.PixKeys?.Any(k => k.Type == "CPF") ?? false;
        }

        if (totalKeysCount >= 20) throw new ConflictError("User already has 20 keys");

        if (hasCpfKey) throw new ConflictError("User already has a CPF key");
    }

    public async Task<PixKeys> GetKeyByValue(string type, string value)
    {
        PixKeys? key = await _keyRepository.GetPixKeyByValue(type, value) ?? throw new NotFoundError("Key not found");
        return key;
    }

}
