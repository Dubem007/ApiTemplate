
namespace Application.Helpers;
public class ResourceParameter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string Search { get; set; }
    public string Sort { get; set; }
    public string FilterBy { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class PayrollResourceParameter : ResourceParameter
{
    public List<string> EmploymentFilter { get; set; } = new();
    public Guid? JobFilter { get; set; }
}


