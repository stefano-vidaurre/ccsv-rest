using System.Linq.Expressions;

namespace CCSV.Rest.Validators;

public class ValidationRule<T,U> : IValidationRule<T>
{
    private Expression<Func<T, U>> _expression { get; }
    private IList<IValidationRuleComponent<U>> _components { get; }

    public string ErrorMessage => throw new NotImplementedException();

    public ValidationRule(Expression<Func<T, U>> expression)
    {
        _expression = expression;
        _components = new List<IValidationRuleComponent<U>>();
    }

    public ValidationRule<T,U> NotNull() {
        _components.Add(new ValidationRuleComponent<U>(value => value != null));
        return this;
    }

    public ValidationRule<T,U> Check(Expression<Func<U, bool>> expression) {
        _components.Add(new ValidationRuleComponent<U>(expression));
        return this;
    }

    public bool Validate(T instance)
    {
        U value = _expression.Compile()(instance);

        return _components.All(component => component.Validate(value));
    }
}