using LanguageExt;
using System;
using static LanguageExt.Prelude;

namespace LanguageExtDemo
{
    enum UserValidationError
    {
        EmailFormatError,
        PhoneFormatError,
        UserNameTooShort,
        UserNameTooLong
    }

    class User
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }


    class ValidationExample
    {
        Validation<UserValidationError, string> ValidatedName(string name) =>
            Fail<UserValidationError, string>(UserValidationError.UserNameTooLong)
            .Disjunction(Fail<UserValidationError, string>(UserValidationError.UserNameTooShort));

        Validation<UserValidationError, string> ValidatedPhone(string phone) =>
            Success<UserValidationError, string>(phone);

        Validation<UserValidationError, string> ValidatedEmail(string phone) =>
            Fail<UserValidationError, string>(UserValidationError.EmailFormatError);

        User User(string name, string email, string phone) => new User
        {
            UserName = name,
            Email = email,
            Phone = phone
        };

        void Example(string name, string email, string phone)
        {
            Validation<UserValidationError, User> user =
                (ValidatedName(name),
                ValidatedEmail(email),
                ValidatedPhone(phone))
                .Apply(User);

            string result = user.Match(
                Succ: u => $"{u.UserName} is valid",
                Fail: erros => $"{erros.Count} errors");
        }
    }
}
