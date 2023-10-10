namespace VebTechTEstApi.ViewModels.UserViewModels;

public class UserFullInfoViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<string>? Roles { get; set; }
}