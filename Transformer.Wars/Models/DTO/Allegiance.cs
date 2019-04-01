using System;
using System.Collections.Generic;

namespace Transformer.Wars.Models.DTO
{
    public class Allegiance
    {
        private List<Models.DB.Transformer> victors = new List<DB.Transformer>();
        private List<Models.DB.Transformer> survivors = new List<DB.Transformer>();
        private List<Models.DB.Transformer> losers = new List<DB.Transformer>();

        public List<Models.DB.Transformer> Losers
        {
            get
            {
                return losers;
            }
            set
            {
                losers = value;
            }
        }


        public List<Models.DB.Transformer> Victors
        {
            get
            {
                return victors;
            }
            set
            {
                victors = value;
            }
        }


        public List<Models.DB.Transformer> Survivors
        {
            get
            {
                return survivors;
            }
            set
            {
                survivors = value;
            }
        }

    }
}
