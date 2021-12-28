using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BokButikLab03.Models
{
    [Table("Förlag")]
    public partial class Förlag
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string Namn { get; set; } = null!;
        [StringLength(50)]
        public string Hemsida { get; set; } = null!;
        [Column("BokISBN13")]
        public long BokIsbn13 { get; set; }

        [ForeignKey(nameof(BokIsbn13))]
        [InverseProperty(nameof(Böcker.Förlags))]
        public virtual Böcker BokIsbn13Navigation { get; set; } = null!;
    }
}
