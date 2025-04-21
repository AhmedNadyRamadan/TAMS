using System;
using System.Collections.Generic;

namespace TASM.Models
{
    public partial class Lab
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int TotalDegree { get; set; }
        public int NoOfSessions { get; set; }
        public DateOnly Date { get; set; }

        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public virtual ICollection<Agenda> Agenda { get; set; } = new List<Agenda>();
        public virtual ICollection<Ta> Ta { get; set; } = new List<Ta>();
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
