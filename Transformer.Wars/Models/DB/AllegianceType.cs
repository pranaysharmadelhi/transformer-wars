using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transformer.Wars.Models.DB
{
    public class AllegianceType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AllegianceTypeId { get; set; }
        public string AllegianceTypeTitle { get; set; }

        public List<Transformer> Transformers { get; set; }

    }
}
