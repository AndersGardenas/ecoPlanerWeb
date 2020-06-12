using econoomic_planer_X.ResourceSet;

namespace econoomic_planer_X.PopulationTypes
{
    public class Farmer : Population
    {
        public Farmer() { }

        public Farmer(int Amount, ResourceTypes.ResourceType producingType, double efficensy) : base(Amount, producingType, efficensy) { }

        public override double GetEfficensy()
        {
            return base.GetEfficensy();
        }
    }
}
