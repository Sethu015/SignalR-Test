using System;
using System.Collections.Generic;

namespace SignalR_Test.EFModels;

public partial class Persons
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
