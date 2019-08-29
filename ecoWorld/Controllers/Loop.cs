using econoomic_planer_X;
using econoomic_planer_X.PopulationTypes;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Server.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ecoPlanerWeb
{
    [Route("api2/[controller]")]
    public class Loop : Controller
    {
        public static void Init(IServiceProvider service)
        {
            using var serviceScope = service.CreateScope();
            var scopeServiceProvider = serviceScope.ServiceProvider;
            EcoContext context = scopeServiceProvider.GetService<EcoContext>();
            Console.WriteLine("Hello World!");
            Demand.Init();
            var sw = new Stopwatch();
            double time = 0;
            int iter = 0;
            sw.Start();
          //  context.ChangeTracker.ValidateOnSaveEnabled = false;
            while (true)
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                IEnumerable<Contry> contries = context.Contry;
                Console.WriteLine("------------------------New gen------------------------------------" + iter++);
                foreach (Contry contry in contries)
                {
                    contry.Update();
                }


                Console.WriteLine("Elapsed Compute ={0}", sw.Elapsed.TotalMilliseconds - time);
                time = sw.Elapsed.TotalMilliseconds;
                context.ChangeTracker.AutoDetectChangesEnabled = true;
                using (var transaction = context.Database.BeginTransaction())
                {
                    context.BulkInsertOrUpdate(contries.ToList());
                    context.BulkInsertOrUpdate(context.Population.ToList());
                    context.BulkInsertOrUpdate(context.Resource.ToList());
                    context.BulkInsertOrUpdate(context.ResourceData.ToList());
                    context.BulkInsertOrUpdate(context.Destination.ToList());
                    context.BulkInsertOrUpdate(context.Region.ToList());


                    transaction.Commit();
                }
                //context.SaveChanges();
                Console.WriteLine("Elapsed Commit ={0}", sw.Elapsed.TotalMilliseconds - time);
                time = sw.Elapsed.TotalMilliseconds;
            }
        }
    }
}
