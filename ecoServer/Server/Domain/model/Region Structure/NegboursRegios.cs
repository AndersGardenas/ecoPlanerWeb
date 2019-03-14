
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace econoomic_planer_X
{
    public class NeighbourRegion
    {

        public Guid Id { get; set; }

        [ForeignKey("Standard")]
        public Region Region1 {get; set; }
        [ForeignKey("Standard")]
        public Region Region2 {get; set; }

        public NeighbourRegion()
        {

        }

        public NeighbourRegion(Region region1, Region region2)
        {
            this.Region1 = region1;
            this.Region2 = region2;
        }
    }
}
