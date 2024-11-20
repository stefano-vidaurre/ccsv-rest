namespace CCSV.Rest.Validators;

public class ValidationResult
{
    private readonly IEnumerable<ValidationRuleResult> _results;
    public bool IsValid => _results.All(result => result.IsValid);

    public IEnumerable<string> Errors => _results.Select(result => result.PrintErrors());

    public ValidationResult()
    {
        _results = new List<ValidationRuleResult>();
    }

    public ValidationResult(IEnumerable<ValidationRuleResult> results)
    {
        _results = results.AsEnumerable();
    }

    public string PrintErrors()
    {
        return _results.Aggregate("", (acc, error) => acc + error.PrintErrors() + '\n');
    }
}