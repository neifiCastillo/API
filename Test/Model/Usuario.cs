using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Test.Model
{
    [Table("Usuario")]
    public partial class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nombre")]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Column("apellido")]
        [StringLength(50)]
        public string Apellido { get; set; }
        [Column("idNumber")]
        [StringLength(50)]
        public string IdNumber { get; set; }
        [Column("fechaNacimiento", TypeName = "date")]
        public DateTime? FechaNacimiento { get; set; }
    }
}
