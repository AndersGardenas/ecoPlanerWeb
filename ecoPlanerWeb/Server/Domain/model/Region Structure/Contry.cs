
using System.Collections.Generic;

namespace econoomic_planer_X
{
    class Contry
    {
        List<Region> regions = new List<Region>();


        void Update()
        {
            
            foreach(Region region in regions){
                region.Update();
            }
        }
    }
}
