using FluentValidation;

namespace Functions.Infrastructure.Pipeline.FluentValidation
{
    public interface IFluentValidatedRequest
    {
        IValidator GetValidator();
    }
}
