using System;
using System.Collections.Generic;

namespace TASM.Models;

public partial class Agenda
{
    public int Id { get; set; }

    public int? LabId { get; set; }

    public int? TaId { get; set; }

    public string TopicName { get; set; } = null!;

    public string Resources { get; set; } = null!;

    public virtual Lab? Lab { get; set; }

    public virtual Ta? Ta { get; set; }
}
