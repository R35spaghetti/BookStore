using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BokButikLab03.Models
{
    [Table("LagerSaldo")]
    public partial class LagerSaldo
    {
        [Key]
        [Column("ButikID")]
        public int ButikId { get; set; }
        [Key]
        [Column("ISBN")]
        public long Isbn { get; set; }
        [Column("ANTAL")]
        public int Antal { get; set; }

        [ForeignKey(nameof(ButikId))]
        [InverseProperty(nameof(Butiker.LagerSaldos))]
        public virtual Butiker Butik { get; set; } = null!;
        [ForeignKey(nameof(Isbn))]
        [InverseProperty(nameof(Böcker.LagerSaldos))]
        public virtual Böcker IsbnNavigation { get; set; } = null!;
    }
}
