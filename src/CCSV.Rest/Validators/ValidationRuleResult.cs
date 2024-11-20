using System.Reflection;

namespace CCSV.Rest.Validators;

public class ValidationRuleResult
{
    private readonly IEnumerable<string> _errors;
    private readonly PropertyInfo _propertyInfo;

    public bool IsValid => !_errors.Any();

    public IEnumerable<string> Errors => _errors.AsEnumerable();

    public ValidationRuleResult(PropertyInfo propertyInfo)
    {
        _errors = new List<string>();
        _propertyInfo = propertyInfo;
    }

    public ValidationRuleResult(PropertyInfo propertyInfo, IEnumerable<string> errors)
    {
        _errors = errors.AsEnumerable();
        _propertyInfo = propertyInfo;
    }

    public string PrintErrors()
    {
        return _errors.Aggregate($"Property '{_propertyInfo.Name}' is not valid: ", (acc, error) => acc + error);
    }
}
