namespace VebTechTEstApi.ViewModels.UserViewModels;

public class IndexViewModel
{
    public Page? Page { get; set; }
    public OrderBy? OrderBy { get; set; }
    public AgeFilter? AgeFilter { get; set; }
    public string? NameFilter { get; set; }
    public string? EmailFilter { get; set; }
    public List<string>? RoleFilter { get; set; }
}

public class Page
{
    public int Number { get; set; }
    public int Size { get; set; }
}
public class OrderBy
{
    public string SortField { get; set; }
    public bool Ascending { get; set; }
}

public class AgeFilter
{
    public int? Min { get; set; }
    public int? Max { get; set; }
}

