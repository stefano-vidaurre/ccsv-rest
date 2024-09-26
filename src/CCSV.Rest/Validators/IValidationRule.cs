namespace CCSV.Rest.Validators;

public interface IValidationRule<in T>
{
    string ErrorMessage { get; }

    bool Validate(T instance);
}