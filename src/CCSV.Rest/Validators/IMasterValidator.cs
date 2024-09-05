using FluentValidation.Results;

namespace CCSV.Rest.Validators;

public interface IMasterValidator
{
    void Validate<T>(T instance);
    Task ValidateAsync<T>(T instance, CancellationToken cancellation = default);
}
