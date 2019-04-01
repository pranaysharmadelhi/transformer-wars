using System;
using System.ComponentModel.DataAnnotations;

namespace Transformer.Wars.Models.DTO
{
    public class TransformerRequest
    {
        public TransformerRequest()
        {
        }


        public TransformerRequest(Models.DB.Transformer transformer)
        {
            _name = transformer.Name;
            _allegianceType = (AllegianceTypes)transformer.AllegianceTypeId;
            _rank = transformer.Rank;
            _strength = transformer.Strength;
            _intelligence = transformer.Intelligence;
            _speed = transformer.Speed;
            _endurance = transformer.Endurance;
            _courage = transformer.Courage;
            _firepower = transformer.Firepower;
            _skill = transformer.Skill;
        }

        public Models.DB.Transformer GetDBObject()
        {
            return new DB.Transformer()
            {
                AllegianceTypeId = (int)_allegianceType,
                Courage = _courage,
                Endurance = _endurance,
                Firepower = _firepower,
                Intelligence = _intelligence,
                Name = _name.Trim(),
                Rank = _rank,
                Skill = _skill,
                Speed = _speed,
                Strength = _strength

            };
        }


               
        private String _name;
        private AllegianceTypes _allegianceType;
        private short _rank;
        private short _strength;
        private short _intelligence;
        private short _speed;
        private short _endurance;
        private short _courage;
        private short _firepower;
        private short _skill;

        [Required]
        [Range(1, 10)]
        public short Skill
        {
            get
            {
                return _skill;
            }
            set
            {
                _skill = value;
            }
        }


        [Required]
        [Range(1, 10)]
        public short Firepower
        {
            get
            {
                return _firepower;
            }
            set
            {
                _firepower = value;
            }
        }


        [Required]
        [Range(1, 10)]
        public short Courage
        {
            get
            {
                return _courage;
            }
            set
            {
                _courage = value;
            }
        }
       

        [Required]
        [Range(1, 10)]
        public short Endurance
        {
            get
            {
                return _endurance;
            }
            set
            {
                _endurance = value;
            }
        }


        [Required]
        [Range(1, 10)]
        public short Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }


        [Required]
        [Range(1, 10)]
        public short Intelligence
        {
            get
            {
                return _intelligence;
            }
            set
            {
                _intelligence = value;
            }
        }

        [Required]
        [Range(1, 10)]
        public short Strength
        {
            get
            {
                return _strength;
            }
            set
            {
                _strength = value;
            }
        }

        [Required]
        [Range(1, 10)]
        public short Rank
        {
            get
            {
                return _rank;
            }
            set
            {
                _rank = value;
            }
        }


        [Required]
        [EnumDataType(typeof(AllegianceTypes))]
        public AllegianceTypes AllegianceType
        {
            get
            {
                return _allegianceType;
            }
            set
            {
                _allegianceType = value;
            }
        }


        [Required]
        [MaxLength(500)]
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

    }
}
