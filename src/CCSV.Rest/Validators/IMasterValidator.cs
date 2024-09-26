namespace CCSV.Rest.Validators;

public interface IMasterValidator
{
    void Validate<T>(T instance);
}
