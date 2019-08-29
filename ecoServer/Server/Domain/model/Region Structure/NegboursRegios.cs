
using System;
using System.ComponentModel.DataAnnotations;

namespace econoomic_planer_X
{
    public class NeighbourRegion
    {

        public int Id { get; set; }

        [Key]
        public virtual Region OwnRegion { get; set; }
        [Key]
        public virtual Region NeighbouringRegion { get; set; }

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
