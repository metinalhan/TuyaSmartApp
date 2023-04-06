using FluentValidation;
using TuyaApp.Application.Dtos.DeviceDtos;

namespace TuyaApp.Application.Validations.Device
{
    //This is validation method for creating devices properties with using Fluent Validation library
    public class CreateDeviceValidator:AbstractValidator<CreateDeviceDTO>
    {
        public CreateDeviceValidator()
        {
            RuleFor(p => p.TuyaAccount).NotNull().WithMessage("Cihaz Atanacak Hesap Seçilmedi");
            RuleFor(p => p.DeviceType).NotNull().NotEqual(-1).WithMessage("Cihaz Türü Seçilmedi");
            RuleFor(p => p.DeviceName).NotNull().NotEmpty().WithMessage("Cihaz Adı Girilmedi");
            RuleFor(p => p.DeviceTuyaId).NotNull().NotEmpty().WithMessage("Cihaz ID Girilmedi");
            RuleFor(p => p.NumberOfSwitch).NotNull().NotEqual(-1).WithMessage("Cihaz Switch Adedi Girilmedi");
        }
    }
}
