
using System;
using System.Collections.Generic;

namespace econoomic_planer_X
{
    public class Contry
    {
        public virtual List<Region> Regions { get; set; }
        public Guid ID { get; set; }
        public string Name {get; set; }

        public Contry(){}
        public Contry(string name) {
            Regions = new List<Region>();
            this.Name = name;
        }

        public void AddRegion(Region region)
        {
            Regions.Add(region);
        }

        public void Update()
        {
            Regions = new List<Region>();
            foreach (Region region in Regions)
            {
                region.Update();
            }
        }
    }
}
