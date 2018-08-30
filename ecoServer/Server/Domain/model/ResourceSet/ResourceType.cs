using System;
using System.ComponentModel.DataAnnotations;

namespace econoomic_planer_X.ResourceSet
{
    public class ResourceType : IComparable, IEquatable<ResourceType>
    {
        public int Id { get; set; }
        public static int totalAmount = 0;
        public string Name { get; set; }

        public ResourceType(string Name)
        {
            Id = totalAmount;
            totalAmount++;
            this.Name = Name;
        }

        public int CompareTo(ResourceType obj)
        {
            return Id.CompareTo(obj.Id);
        }

        public int CompareTo(object obj)
        {
            return CompareTo((ResourceType)obj);
        }

        public bool Equals(ResourceType other)
        {
            return CompareTo(other) == 0;
        }
    }
}
