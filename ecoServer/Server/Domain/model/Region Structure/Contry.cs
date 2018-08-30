
using System;
using System.Collections.Generic;

namespace econoomic_planer_X
{
    public class Contry
    {
        public virtual List<Region> Regions { get; set; }
        public Guid ID { get; set; }

        public Contry(){}

        void Update()
        {
            Regions = new List<Region>();
            foreach (Region region in Regions)
            {
                region.Update();
            }
        }
    }
}
