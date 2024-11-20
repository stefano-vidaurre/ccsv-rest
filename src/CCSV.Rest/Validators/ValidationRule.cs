using CCSV.Domain.Exceptions;
using System.Linq.Expressions;
using System.Reflection;

namespace CCSV.Rest.Validators;

public class ValidationRule<T,U> : IValidationRule<T>
{
    private readonly PropertyInfo _propertyInfo;
    private readonly Expression<Func<T, U>> _expression;
    private readonly IList<IValidationRuleComponent<U>> _components;

    public string ErrorMessage => throw new NotImplementedException();

    public ValidationRule(Expression<Func<T, U>> expression)
    {
        if (expression.Body is not MemberExpression member)
        {
            throw new WrongOperationException($"Expression '{expression.ToString()}' refers to a method, not a property.");
        }

        if (member.Member is not PropertyInfo propertyInfo)
        {
            throw new WrongOperationException($"Expression '{expression.ToString()}' refers to a field, not a property.");
        }

        _propertyInfo = propertyInfo;
        _expression = expression;
        _components = new List<IValidationRuleComponent<U>>();
    }

    public ValidationRule<T,U> NotNull() {
        _components.Add(new ValidationRuleComponent<U>(value => value != null, "Value cant be null."));
        return this;
    }

    public ValidationRule<T, U> NotNull(string message)
    {
        _components.Add(new ValidationRuleComponent<U>(value => value != null, message));
        return this;
    }

    public ValidationRule<T,U> NotEmpty() {
        _components.Add(new ValidationRuleComponent<U>(value => !string.IsNullOrEmpty(value == null ? null : value.ToString()), "Value cant be empty."));
        return this;
    }

    public ValidationRule<T, U> NotEmpty(string message)
    {
        _components.Add(new ValidationRuleComponent<U>(value => !string.IsNullOrEmpty(value == null ? null : value.ToString()), message));
        return this;
    }

    public ValidationRule<T,U> NotWhiteSpace() {
        _components.Add(new ValidationRuleComponent<U>(value => !string.IsNullOrWhiteSpace(value == null ? null : value.ToString()), "Value cant be empty or white space."));
        return this;
    }

    public ValidationRule<T, U> NotWhiteSpace(string message)
    {
        _components.Add(new ValidationRuleComponent<U>(value => !string.IsNullOrWhiteSpace(value == null ? null : value.ToString()), message));
        return this;
    }

    public ValidationRule<T,U> Check(Expression<Func<U, bool>> expression) {
        _components.Add(new ValidationRuleComponent<U>(expression));
        return this;
    }

    public ValidationRule<T, U> Check(Expression<Func<U, bool>> expression, string message)
    {
        _components.Add(new ValidationRuleComponent<U>(expression, message));
        return this;
    }

    public ValidationRuleResult Validate(T instance)
    {
        U value = _expression.Compile()(instance);
        
        IEnumerable<string> errorMessages = _components.Where(component => !component.Validate(value)).Select(component => component.ErrorMessage);

        return new ValidationRuleResult(_propertyInfo, errorMessages);
    }
}