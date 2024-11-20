using CCSV.Domain.Exceptions;
using System.Reflection;

namespace CCSV.Rest.Validators;

public class MasterValidator : IMasterValidator
{
    private readonly IDictionary<Type, Type> _validators;

    public MasterValidator(params Type[] validators) : this(validators.AsEnumerable())
    {

    }

    public MasterValidator(IEnumerable<Type> validators)
    {
        _validators = new Dictionary<Type, Type>();
        foreach (Type validator in validators)
        {
            Type? baseType = validator.BaseType;
            if (baseType is null)
            {
                throw new BusinessException("Error in master validator, there are some validators that doesn't implement Validator<T>.");
            }

            if (!baseType.IsGenericType)
            {
                throw new BusinessException("Error in master validator, there are some validators that doesn't implement Validator<T>.");
            }

            if (baseType.GetGenericTypeDefinition() != typeof(Validator<>))
            {
                throw new BusinessException("Error in master validator, there are some validators that doesn't implement Validator<T>.");
            }

            Type genericArgumentType = baseType.GetGenericArguments()[0];

            _validators.Add(genericArgumentType, validator);
        }
    }

    public static MasterValidator CreateByAssembly(params Assembly[] assemblies)
    {
        return CreateByAssembly(assemblies.AsEnumerable());
    }

    public static MasterValidator CreateByAssembly(IEnumerable<Assembly> assemblies)
    {
        List<Type> types = new List<Type>();

        foreach (Assembly assembly in assemblies)
        {
            IEnumerable<Type> validators = assembly.GetTypes()
                .Where(type => type.BaseType?.IsGenericType ?? false)
                .Where(type => type.BaseType?.GetGenericTypeDefinition() == typeof(Validator<>));

            types.AddRange(validators);
        }

        return new MasterValidator(types);
    }

    public void Validate<T>(T instance)
    {
        if (!_validators.TryGetValue(typeof(T), out var validatorType))
        {
            throw new WrongOperationException($"Class ({typeof(T).Name}) has not a validator.");
        }

        IValidator<T> validator = validatorType.GetConstructor(Type.EmptyTypes)?.Invoke(null) as IValidator<T> ?? throw new BusinessException("The validator type is not valid.");

        ValidationResult result = validator.Validate(instance);

        if (!result.IsValid)
        {
            throw new InvalidValueException(result.PrintErrors());
        }
    }
}
