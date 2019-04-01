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

            modelBuilder.Entity<AllegianceType>(entity => {
                entity.HasData(
                  new AllegianceType { AllegianceTypeId = (int)Models.AllegianceTypes.Autobot , AllegianceTypeTitle = Models.AllegianceTypes.Autobot.ToString() },
                  new AllegianceType { AllegianceTypeId = (int)Models.AllegianceTypes.Decepticon , AllegianceTypeTitle = Models.AllegianceTypes.Decepticon.ToString() }
                );
            });

            modelBuilder.Entity<Transformer>()
                .HasIndex(b => b.AllegianceTypeId);                
        }
    }
}
