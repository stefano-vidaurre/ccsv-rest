using System.Linq.Expressions;

namespace CCSV.Rest.Validators;

public class ValidationRuleComponent<U> : IValidationRuleComponent<U>
{
    public Expression<Func<U, bool>> Expression { get; }
    public string ErrorMessage { get; private set; }

    public ValidationRuleComponent(Expression<Func<U, bool>> expression)
    {
        Expression = expression;
        ErrorMessage = string.Empty;
    }

    public ValidationRuleComponent(Expression<Func<U, bool>> expression, string errorMessage)
    {
        Expression = expression;
        ErrorMessage = errorMessage;
    }

    public bool Validate(U value)
    {
        return Expression.Compile()(value);
    }
}
