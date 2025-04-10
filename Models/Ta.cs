using System;
using System.Collections.Generic;

namespace TASM.Models;

public partial class Ta
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? LabId { get; set; }

    public virtual ICollection<Agenda> Agenda { get; set; } = new List<Agenda>();

    public virtual Lab? Lab { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
