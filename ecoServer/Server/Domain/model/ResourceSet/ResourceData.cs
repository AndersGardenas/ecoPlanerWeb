using econoomic_planer_X.ResourceSet;
using System;

namespace Server.Server.Domain.model.ResourceSet
{
    public class ResourceData
    {

        public int Id { get; set; }
        public virtual double ResourcesPrice { get; set; }
        public virtual double ResourceRatio { get; set; }
        public virtual ResourceTypes.ResourceType ResourceType { get; set; }

        public virtual int InternalMarketId { get; set; }

        public ResourceData()
        {
            ResourcesPrice = 1;
        }

        public ResourceData(ResourceTypes.ResourceType resourceType): this()
        {
            ResourceType = resourceType;
        }

    }
}
