using Microsoft.EntityFrameworkCore;

namespace ecoWorld.Resources
{
    public static class DbExtensionscs
    {
        public static void DeleteAll<T>(this DbContext context)
         where T : class
        {
            foreach (var p in context.Set<T>())
            {
                context.Entry(p).State = EntityState.Deleted;
            }
        }
    }
}
