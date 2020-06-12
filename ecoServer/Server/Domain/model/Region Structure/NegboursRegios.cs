using System.ComponentModel.DataAnnotations.Schema;

namespace econoomic_planer_X
{
    public class NeighbourRegion
    {
        public int Id { get; set; }

        [InverseProperty("OwnRegion")]

        public virtual Region OwnRegion { get; set; }
        [InverseProperty("NeighbouringRegion")]
        public virtual Region NeighbouringRegion { get; set; }

        public NeighbourRegion()
        {
        }

        public NeighbourRegion(Region OwnRegion, Region NeighbouringRegion)
        {
            if (OwnRegion.RegionID > NeighbouringRegion.RegionID)
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
