using FluentValidation;
using TokenBasedAuthApplication.Core.DTOs;

namespace TokenBasedAuthApplication.Business.Validations;

public class CreateUserValidatior: AbstractValidator<CreateUserDto>
{
    public CreateUserValidatior()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("E-posta adresi boş bırakılamaz!")
            .NotEmpty().WithMessage("E-Posta adresi boş bırakılamaz!")
            .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi giriniz.");
        
        RuleFor(x => x.UserName)
            .NotNull().WithMessage("Kullanıcı adı boş bırakılamaz!")
            .NotEmpty().WithMessage("Kullanıcı adı boş bırakılamaz!");
        
        RuleFor(x => x.Password)
            .NotNull().WithMessage("Parola boş bırakılamaz!")
            .NotEmpty().WithMessage("Parola adresi boş bırakılamaz!");
    }
}