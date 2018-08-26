using System;

namespace Server.Server.Domain.model.ResourceSet
{
    public class ResourceData
    {

        public Guid Id { get; set; }
        public virtual double ResourcesPrice { get; set; }

        public virtual double ResourceRatio { get; set; }

        ResourceData()
        {
            ResourcesPrice = 1;
        }


    }
}
