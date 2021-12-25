using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BokButikLab03.Models
{
    [Table("Böcker")]
    public partial class Böcker
    {
        public Böcker()
        {
            LagerSaldos = new HashSet<LagerSaldo>();
            Författares = new HashSet<Författare>();
        }

        [Key]
        [Column("ISBN13")]
        public long Isbn13 { get; set; }
        [StringLength(50)]
        public string Titel { get; set; } = null!;
        [StringLength(50)]
        public string Språk { get; set; } = null!;
        [Column(TypeName = "money")]
        public decimal Pris { get; set; }
        [Column(TypeName = "date")]
        public DateTime Utgivningsdatum { get; set; }

        [InverseProperty(nameof(LagerSaldo.IsbnNavigation))]
        public virtual ICollection<LagerSaldo> LagerSaldos { get; set; }

        [ForeignKey("BöckerIsbn")]
        [InverseProperty(nameof(Författare.BöckerIsbns))]
        public virtual ICollection<Författare> Författares { get; set; }
    }
}
