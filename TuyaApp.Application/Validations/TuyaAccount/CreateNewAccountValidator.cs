using FluentValidation;
using TuyaApp.Application.Dtos.TuyaAccountDtos;

namespace TuyaApp.Application.Validations.TuyaAccount
{
    //This is validation method for creating account properties with using Fluent Validation library
    public class CreateNewAccountValidator: AbstractValidator<CreateNewAccountDTO>
    {
        public CreateNewAccountValidator()
        {
            RuleFor(p => p.AccountName).NotNull().NotEmpty().WithMessage("Hesap Adı Girişi Yapılmadı");
            RuleFor(p => p.ClientId).NotNull().NotEmpty().WithMessage("Client ID Girişi Yapılmadı");
            RuleFor(p => p.Secret).NotNull().NotEmpty().WithMessage("Secret Girişi Yapılmadı");
        }
    }
}
