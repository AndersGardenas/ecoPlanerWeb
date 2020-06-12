using econoomic_planer_X.ResourceSet;

namespace econoomic_planer_X.PopulationTypes
{
    public class Artisans : Population
    {
        public Artisans() { }

        public Artisans(int Amount, ResourceTypes.ResourceType producingType) : base(Amount, producingType, 1) { }


        public override double GetEfficensy()
        {
            return base.GetEfficensy() * 2;
        }
    }
}
