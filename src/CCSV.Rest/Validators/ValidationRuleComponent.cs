using System.Linq.Expressions;

namespace CCSV.Rest.Validators;

public class ValidationRuleComponent<U> : IValidationRuleComponent<U>
{
    public Expression<Func<U, bool>> Expression { get; }

    public ValidationRuleComponent(Expression<Func<U, bool>> expression)
    {
        Expression = expression;
    }

    public bool Validate(U value)
    {
        return Expression.Compile()(value);
    }
}
