using System.Runtime.CompilerServices;

namespace VebTechTEstApi.Models;

public class DateHistory
{
    public bool IsDeleted { get; set; } = false;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

}