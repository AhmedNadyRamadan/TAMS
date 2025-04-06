using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TASM.Models;

public partial class TamsContext : DbContext
{
    public TamsContext()
    {
    }

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=TAMS;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agenda>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Agendas__3214EC07D669FB55");

            entity.HasIndex(e => e.TopicName, "UQ__Agendas__6C795E8C00D7611B").IsUnique();

            entity.Property(e => e.LabId).HasColumnName("LabID");
            entity.Property(e => e.Resources).IsUnicode(false);
            entity.Property(e => e.TaId).HasColumnName("TA_ID");
            entity.Property(e => e.TopicName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Lab).WithMany(p => p.Agenda)
                .HasForeignKey(d => d.LabId)
                .HasConstraintName("FK_Agenda_Lab");

            entity.HasOne(d => d.Ta).WithMany(p => p.Agenda)
                .HasForeignKey(d => d.TaId)
                .HasConstraintName("FK_Agenda_TA");
        });

        modelBuilder.Entity<Lab>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Labs__3214EC074B833EDC");

            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasMany(d => d.Students).WithMany(p => p.Labs)
                .UsingEntity<Dictionary<string, object>>(
                    "LabsStudent",
                    r => r.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Labs_Students_Student"),
                    l => l.HasOne<Lab>().WithMany()
                        .HasForeignKey("LabId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Labs_Students_Lab"),
                    j =>
                    {
                        j.HasKey("LabId", "StudentId");
                        j.ToTable("Labs_Students");
                    });
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sessions__3214EC0781BFC7A6");

            entity.Property(e => e.LabId).HasColumnName("LabID");

            entity.HasOne(d => d.Lab).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.LabId)
                .HasConstraintName("FK_Session_Lab");
        });

        modelBuilder.Entity<SessionsStudent>(entity =>
        {
            entity.HasKey(e => new { e.SessionId, e.StudentId });

            entity.ToTable("Sessions_Students");

            entity.Property(e => e.AttendanceDegree).HasColumnName("Attendance_Degree");

            entity.HasOne(d => d.Session).WithMany(p => p.SessionsStudents)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sessions_Students_Lab");

            entity.HasOne(d => d.Student).WithMany(p => p.SessionsStudents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sessions_Students_Student");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC07C4EF6A14");

            entity.HasIndex(e => e.Email, "UQ__Students__A9D10534EB311CE8").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasMany(d => d.Ta).WithMany(p => p.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentsTa",
                    r => r.HasOne<Ta>().WithMany()
                        .HasForeignKey("TaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Students_TAs_TA"),
                    l => l.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Students_TAs_Student"),
                    j =>
                    {
                        j.HasKey("StudentId", "TaId");
                        j.ToTable("Students_TAs");
                        j.IndexerProperty<int>("TaId").HasColumnName("TA_Id");
                    });
        });

        modelBuilder.Entity<Ta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TAs__3214EC07CBD5634C");

            entity.ToTable("TAs");

            entity.HasIndex(e => e.Email, "UQ__TAs__A9D105347DF4B77D").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LabId).HasColumnName("LabID");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Lab).WithMany(p => p.Ta)
                .HasForeignKey(d => d.LabId)
                .HasConstraintName("FK_TA_Lab");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
