using econoomic_planer_X;
using econoomic_planer_X.PopulationTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Server.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ecoPlanerWeb
{
    [Route("api2/[controller]")]
    public class GameLoop : Controller
    {
        public static void Init(IServiceProvider service)
        {
            using var serviceScope = service.CreateScope();
            var scopeServiceProvider = serviceScope.ServiceProvider;
            EcoContext context = scopeServiceProvider.GetService<EcoContext>();
            Demand.Init();
            var sw = new Stopwatch();
            double time = 0;
            int iter = 0;
            sw.Start();
            //  context.ChangeTracker.ValidateOnSaveEnabled = false;
            while (true)
            {
                double startTime = sw.Elapsed.TotalMilliseconds;
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
                CleanUp(context);
                context.BulkSaveChanges();
                Console.WriteLine("Elapsed Commit ={0}", sw.Elapsed.TotalMilliseconds - time);
                time = sw.Elapsed.TotalMilliseconds;
                Thread.Sleep((int)Math.Max(1,1000 - (sw.Elapsed.TotalMilliseconds - startTime)));
            }
        }
        public static void CleanUp(EcoContext context)
        {
            context.TradingResource.RemoveRange(context.TradingResource.Where(r => r.Owner == null));
            //context.ExternalTradingResources.RemoveRange(context.TradingResource.Where(r => r.Owner == null));
        }
    }

}
