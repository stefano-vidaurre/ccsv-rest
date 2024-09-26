using System.Linq.Expressions;

namespace CCSV.Rest.Validators;

public abstract class Validator<T> : IValidator<T>
{
    private readonly IList<IValidationRule<T>> _rules;

    protected Validator()
    {
        _rules = new List<IValidationRule<T>>();
    }

    public ValidationRule<T,U> RuleFor<U>(Expression<Func<T, U>> expression)
    {
        ValidationRule<T, U> rule = new ValidationRule<T, U>(expression);
        _rules.Add(rule);
        return rule;
    }

    public bool CanValidateInstancesOfType(Type type)
    {
        return typeof(T) == type;
    }

    public ValidationResult Validate(T instance)
    {
        IEnumerable<IValidationRule<T>> failedRules = _rules.Where(rule => !rule.Validate(instance));
        ValidationResult result = new ValidationResult(failedRules.Select(rule => rule.ErrorMessage));
        return result;
    }
}