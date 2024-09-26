namespace CCSV.Rest.Validators;

public interface IValidationRuleComponent<in U>
{
    bool Validate(U value);
}
