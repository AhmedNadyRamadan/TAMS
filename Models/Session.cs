using System;
using System.Collections.Generic;

namespace TASM.Models;

public partial class Session
{
    public int Id { get; set; }

    public int? LabId { get; set; }

    public DateOnly Date { get; set; }

    public virtual Lab? Lab { get; set; }

    public virtual ICollection<SessionsStudent> SessionsStudents { get; set; } = new List<SessionsStudent>();
}
