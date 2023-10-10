namespace VebTechTEstApi.ViewModels.UserViewModels;

public class UserListViewModel
{
    public int TotalItems { get; set; }
    public ICollection<UserViewModel>? Users { get; set; }
    
}

public class UserViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public List<string>? Roles { get; set; }
}