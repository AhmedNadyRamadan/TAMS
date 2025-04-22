using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TASM.Models;

namespace TASM.Data;

public partial class TamsContext : IdentityDbContext<IdentityUser>
{
    public TamsContext(DbContextOptions<TamsContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Agenda> Agendas { get; set; }

    public virtual DbSet<Lab> Labs { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<SessionsStudent> SessionsStudents { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Ta> Tas { get; set; }
}

