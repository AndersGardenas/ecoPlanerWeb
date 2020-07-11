using econoomic_planer_X;
using econoomic_planer_X.PopulationTypes;
using econoomic_planer_X.ResourceSet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Server.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ecoPlanerWeb
{
    [Route("api2/[controller]")]
    public class GameLoop : Controller
    {
        public static void Init(IServiceProvider service)
        {
            Init();

            using var serviceScope = service.CreateScope();
            var scopeServiceProvider = serviceScope.ServiceProvider;
            EcoContext context = scopeServiceProvider.GetService<EcoContext>();
            var sw = new Stopwatch();
            double time = 0;
            int iter = 0;
            sw.Start();
            int frameTime = 100;
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
                Thread.Sleep((int)Math.Max(1, frameTime - (sw.Elapsed.TotalMilliseconds - startTime)));
                time = sw.Elapsed.TotalMilliseconds;
            }
        }
        public static void CleanUp(EcoContext context)
        {
            IQueryable<TradingResource> tr = context.TradingResource.Where(r => r.TradingResourcesID == null && r.ExternalTradingResourcesID == null);
            context.Destination.RemoveRange(context.Destination.Where(dr => dr.ExternatlTradingResource == null));
            context.TradingResource.RemoveRange(tr);
            //context.Destination.RemoveRange(context.Destination.Where(d => d.DaysRemaning <= 0));
        }

        public static void Init()
        {
            string CultureName = Thread.CurrentThread.CurrentCulture.Name;
            CultureInfo ci = new CultureInfo(CultureName);
            if (ci.NumberFormat.NumberDecimalSeparator != ".")
            {
                // Forcing use of decimal separator for numerical values
                ci.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture = ci;
            }
            CultureInfo.CurrentCulture = new CultureInfo("en-GB", false);
        }
    }

}
