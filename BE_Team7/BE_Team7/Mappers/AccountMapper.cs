using api.Dtos.Account;

namespace api.Mappers
{
    public static class AccountMapper
    {
        public static AccountDto ToAccountDto(this User user)
        {
            return new AccountDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }

    }
}