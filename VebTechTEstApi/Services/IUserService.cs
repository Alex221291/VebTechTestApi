using VebTechTEstApi.Data;

namespace VebTechTEstApi.Services;

public interface IUserService
{
    
}

public class UserService: IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }
}