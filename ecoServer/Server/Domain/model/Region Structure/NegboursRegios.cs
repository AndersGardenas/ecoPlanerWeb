
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace econoomic_planer_X
{
    public class NeighbourRegion
    {

        public Guid Id { get; set; }

        [Key]
        public Region OwnRegion { get; set; }
        [Key]
        public Region NeighbouringRegion { get; set; }

        public NeighbourRegion()
        {

        }

        public NeighbourRegion(Region OwnRegion, Region NeighbouringRegion)
        {
            if (OwnRegion.ID.CompareTo(NeighbouringRegion.ID) > 0)
            {
                this.OwnRegion = OwnRegion;
                this.NeighbouringRegion = NeighbouringRegion;
            }
            else
            {
                this.OwnRegion = NeighbouringRegion;
                this.NeighbouringRegion = OwnRegion;
            }
        }
    }
}
