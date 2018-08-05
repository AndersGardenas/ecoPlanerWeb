using econoomic_planer_X.ResourceSet;
using System;
using System.Collections.Generic;
using System.Text;

namespace econoomic_planer_X.PopulationTypes
{
    public class Farmer : Population
    {
     

        public Farmer(int Amount,ResourceType producingType): base(Amount,producingType){ }

        public override double getEfficensy()
        {
            return base.getEfficensy();
        }

    }
}
