using econoomic_planer_X.ResourceSet;

namespace econoomic_planer_X.PopulationTypes
{
    public class Farmer : Population
    {
        public Farmer(){}

        public Farmer(int Amount,ResourceType producingType): base(Amount,producingType){ }

        public override double GetEfficensy()
        {
            return base.GetEfficensy();
        }
    }
}
