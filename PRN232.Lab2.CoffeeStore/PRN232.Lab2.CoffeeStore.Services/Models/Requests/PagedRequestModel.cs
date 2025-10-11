namespace PRN232.Lab2.CoffeeStore.Services.Models.Requests;

public class PagedRequestModel
{
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public string? Select { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
