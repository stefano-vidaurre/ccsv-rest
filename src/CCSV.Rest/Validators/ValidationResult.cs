namespace CCSV.Rest.Validators;

public class ValidationResult
{
    private readonly IEnumerable<string> _errors;
    public bool IsValid => !_errors.Any();

    public IEnumerable<string> Errors => _errors.AsEnumerable();

    public ValidationResult()
    {
        _errors = new List<string>();
    }

    public ValidationResult(IEnumerable<string> errors)
    {
        _errors = errors.AsEnumerable();
    }

    public string PrintErrors()
    {
        return _errors.Aggregate("", (acc, error) => acc + error + '\n');
    }
}