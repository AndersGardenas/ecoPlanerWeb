using econoomic_planer_X;
using econoomic_planer_X.ResourceSet;
using Microsoft.AspNetCore.Mvc;
using Server.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ecoPlanerWeb.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {

        protected EcoContext context;
        public SampleDataController(EcoContext context) {
            this.context = context;

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            Init(context);
            context.SaveChanges();
        }

        
            public static void Init(EcoContext context) {
            Console.WriteLine("Hello World!");
            ResourceTypes.Init();

            var op = new Region(true);
            var op2 = new Region(false);
            var regions = new List<Region>();
            regions.Add(op);
            regions.Add(op2);
            context.Region.Add(op);
            context.Region.Add(op2);

            //op.Connect(op2);

             //int iter = 0;
            //while (true)
            //{
            //    Console.WriteLine("------------------------New gen------------------------------------" + iter++);
            //    regions.ForEach(re => re.Update());

            //    regions.ForEach(re => re.CleanUp());
            //    System.Threading.Thread.Sleep(10);
            //}
        }


        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public String getContry(int index) {
            var rng = new Random();
            return context.Population;
        }


        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts(int startDateIndex) {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                DateFormatted = DateTime.Now.AddDays(index + startDateIndex).ToString("d"),
                TemperatureC = Convert.ToInt16(context.Contry.First(c => true).ID.ToString()),
                //rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF {
                get {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
