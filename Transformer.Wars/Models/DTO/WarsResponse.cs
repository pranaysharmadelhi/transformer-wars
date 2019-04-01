using System;
using System.Collections.Generic;

namespace Transformer.Wars.Models.DTO
{
    public class WarsResponse
    {
        private Allegiance autobots = new Allegiance();
        private Allegiance decepticon = new Allegiance();

        private List<String> battles = new List<string>();

        public List<String> Battles
        {
            get
            {
                return battles;
            }
            set
            {
                battles = value;
            }
        }


        public Allegiance Autobots
        {
            get
            {
                return autobots;
            }
            set
            {
                autobots = value;
            }
        }


        public Allegiance Decepticon
        {
            get
            {
                return decepticon;
            }
            set
            {
                decepticon = value;
            }
        }



    }
}
