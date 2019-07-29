using LanguageExt;
using System.Web.Http;
using static LanguageExt.Prelude;

namespace LanguageExtDemo
{
    enum AccountError
    {
        AccountNotFound,
        AccessDenied
    }

    class EitherExample : ApiController
    {
        Either<AccountError, uint> ExistingAccountId(uint accountId) => Right(accountId);
        Either<AccountError, decimal> AccountBalance(uint accountId) => Left(AccountError.AccessDenied);

        Either<AccountError, decimal> AccountBalanceById(uint accountId)
        {
            Either<AccountError, uint> existingAccountId = ExistingAccountId(accountId);
            Either<AccountError, decimal> accountBalance = existingAccountId.Bind(AccountBalance);

            return accountBalance;
        }

        IHttpActionResult GetAccountBalanceById(uint accountId)
        {
            var balance = AccountBalanceById(accountId);

            var result = balance.Match(
                Right: Ok,
                Left: erorr => erorr switch
                {
                    AccountError.AccountNotFound => (IHttpActionResult) NotFound(),
                    AccountError.AccessDenied => Unauthorized()
                });

            return result;
        }
    }
}