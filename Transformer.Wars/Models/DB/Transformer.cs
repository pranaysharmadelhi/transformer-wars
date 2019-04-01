using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Transformer.Wars.Models.DB
{
    public class Transformer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransformerId { get; set; }
        [Required]
        [JsonIgnore]
        public int AllegianceTypeId { get; set; }
        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        public AllegianceTypes Allegiance { get { return (AllegianceTypes)AllegianceTypeId; } }
        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonIgnore]
        public int Score { get { return Strength + Intelligence + Speed + Endurance + Courage + Firepower + Skill + Rank; } }
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
        [JsonIgnore]
        public AllegianceType AllegianceType { get; set; }

    }
}
