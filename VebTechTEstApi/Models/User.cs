﻿namespace VebTechTEstApi.Models;

public class User: DateHistory
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public ICollection<Role>? Roles { get; set; }
}