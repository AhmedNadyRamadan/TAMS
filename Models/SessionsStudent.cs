using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TASM.Models;

[PrimaryKey(nameof(SessionId),nameof(StudentId))]
public partial class SessionsStudent
{
    public int SessionId { get; set; }

    public int StudentId { get; set; }

    public int? AttendanceDegree { get; set; }

    public virtual Session Session { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
