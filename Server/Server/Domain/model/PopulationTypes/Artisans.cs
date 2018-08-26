using econoomic_planer_X.ResourceSet;

namespace econoomic_planer_X.PopulationTypes
{
    public class Artisans : Population
    {
        public Artisans() { }

        public Artisans(int Amount, ResourceType producingType) : base(Amount, producingType) { }


        public override double GetEfficensy() {
            return base.GetEfficensy() * 2;
        }
    }
}
