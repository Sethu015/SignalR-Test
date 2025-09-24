using System;
using System.Collections.Generic;

namespace SignalR_Test.EFModels;

public partial class Connections
{
    public Guid Id { get; set; }

    public Guid PersonId { get; set; }

    public string SignalrId { get; set; } = null!;

    public DateTime Timestamp { get; set; }
}
