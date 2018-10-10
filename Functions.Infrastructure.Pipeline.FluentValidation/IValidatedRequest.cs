using FluentValidation;

namespace Functions.Infrastructure.Pipeline.FluentValidation
{
    public interface IValidatedRequest
    {
        IValidator GetValidator();
    }
}
