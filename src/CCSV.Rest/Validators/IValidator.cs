namespace CCSV.Rest.Validators;

public interface IValidator<in T> {
    bool CanValidateInstancesOfType(Type type);
    ValidationResult Validate(T instance);
}