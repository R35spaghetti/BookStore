using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BokButikLab03.Models
{
    [Table("Författare")]
    public partial class Författare
    {
        public Författare()
        {
            BöckerIsbns = new HashSet<Böcker>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string Förnamn { get; set; } = null!;
        [StringLength(50)]
        public string Efternamn { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime Födelsedatum { get; set; }

        [ForeignKey("FörfattareId")]
        [InverseProperty(nameof(Böcker.Författares))]
        public virtual ICollection<Böcker> BöckerIsbns { get; set; }
    }
}
