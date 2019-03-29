using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Transformer.Wars.Models.DB
{
    public class TransformerWarsContext : DbContext
    {
        public TransformerWarsContext(DbContextOptions<TransformerWarsContext> options)
            : base(options)
        { }

        public DbSet<Transformer> Transformers { get; set; }
        public DbSet<AllegianceType> AllegianceTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transformer>()
                .HasIndex(b => b.AllegianceTypeId);

            modelBuilder.Entity<Transformer>(entity =>
            {
                entity.HasKey(e => e.AllegianceTypeId);
            });

            modelBuilder.Entity<AllegianceType>().HasData(
        new AllegianceType { AllegianceTypeId = 1, AllegianceTypeTitle = "Autobot" },
        new AllegianceType { AllegianceTypeId = 2, AllegianceTypeTitle = "Decepticon" }
    );
        }
    }

    public class Transformer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransformerId { get; set; }
        [Required]
        public int AllegianceTypeId { get; set; }
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
        [Required]
        public short Rank { get; set; }
        [Required]
        public short Strength { get; set; }
        [Required]
        public short Intelligence { get; set; }
        [Required]
        public short Speed { get; set; }
        [Required]
        public short Endurance { get; set; }
        [Required]
        public short Courage { get; set; }
        [Required]
        public short Firepower { get; set; }
        [Required]
        public short Skill { get; set; }

        [ForeignKey("AllegianceTypeId")]
        public AllegianceType AllegianceType { get; set; }

    }

    public class AllegianceType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AllegianceTypeId { get; set; }
        public string AllegianceTypeTitle { get; set; }

        public List<Transformer> Transformers { get; set; }

    }

}
