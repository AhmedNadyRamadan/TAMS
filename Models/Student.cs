using System;
using System.Collections.Generic;

namespace TASM.Models;

public partial class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<SessionsStudent> SessionsStudents { get; set; } = new List<SessionsStudent>();

    public virtual ICollection<Lab> Labs { get; set; } = new List<Lab>();

    public virtual ICollection<Ta> Ta { get; set; } = new List<Ta>();
}
