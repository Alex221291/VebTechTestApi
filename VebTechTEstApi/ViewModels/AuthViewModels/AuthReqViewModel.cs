namespace VebTechTEstApi.ViewModels.AuthViewModels;

public class AuthReqViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public string? Token { get; set; }
}