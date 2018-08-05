using econoomic_planer_X.ResourceSet;
using System;
using System.Collections.Generic;
using System.Text;

namespace econoomic_planer_X.PopulationTypes
{
    public class Artisans : Population
    {
     
          public Artisans(int Amount, ResourceType producingType): base(Amount,producingType){ }


        public override double getEfficensy()
        {
            return base.getEfficensy() * 2;
        }
    }
}
