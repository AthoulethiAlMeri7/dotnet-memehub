
using api.Application.Dtos;
using api.Application.Services.ServiceContracts;
using FluentValidation;

namespace api.Application.validators
{
    public class CreateMemeDtoValidator : AbstractValidator<CreateMemeDto>
    {
        public CreateMemeDtoValidator(ITemplateService templateService)
        {
            RuleFor(x => x.TemplateId)
                .NotEmpty()
                .WithMessage("TemplateId is required.")
                .MustAsync(async (templateId, cancellation) =>
                    await templateService.TemplateExists(templateId))
                .WithMessage("The specified TemplateId does not exist.");
        }
    }
}