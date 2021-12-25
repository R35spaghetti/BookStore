using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BokButikLab03.Models
{
    [Table("Butiker")]
    public partial class Butiker
    {
        public Butiker()
        {
            LagerSaldos = new HashSet<LagerSaldo>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string Butiksnamn { get; set; } = null!;

        [InverseProperty(nameof(LagerSaldo.Butik))]
        public virtual ICollection<LagerSaldo> LagerSaldos { get; set; }
    }
}
