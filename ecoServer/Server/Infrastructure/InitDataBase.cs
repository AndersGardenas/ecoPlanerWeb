
using econoomic_planer_X;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace Server.Server.Infrastructure
{
    public class InitDataBase
    {
        public static void InitDB(IServiceProvider service)
        {
            using IServiceScope serviceScope = service.CreateScope();
            var scopeServiceProvider = service.CreateScope().ServiceProvider;
            EcoContext context = scopeServiceProvider.GetService<EcoContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.SaveChanges();

            var contries = new List<Contry>();
            Console.WriteLine("Hello World!");
            using (StreamReader file = File.OpenText(@"C:\Users\Anders\Source\Repos\ecoPlanerWeb\ecoWorld\ClientApp\src\custom.geo.json"))
            {
                var random = new Random();
                using (var reader = new JsonTextReader(file))
                {
                    var o2 = (JObject)JToken.ReadFrom(reader);
                    o2.GetValue("features");
                    foreach (JToken token in o2.GetValue("features"))
                    {
                        string contryName = token["properties"]["admin"].ToString();
                        var contry = new Contry(contryName);
                        RegionCompute(random, token, contry);
                        context.Contry.Add(contry);
                        contries.Add(contry);
                    }
                }
            }

            //for (int i = 0; i < contries.Count; i++)
            //{
            //    for (int j = i + 1; j < contries.Count; j++)
            //    {
            //        foreach (Region region1 in contries[i].Regions)
            //        {
            //            foreach (Region region2 in contries[j].Regions)
            //            {
            //                bool intersect = 10 > Math.Sqrt(Math.Pow(region1.CenterX - region2.CenterX, 2) + Math.Pow(region1.CenterY - region2.CenterY, 2));
            //                //PointCollectionsOverlap_Fast(new PointCollection(region1.GetPolygon()), new PointCollection(region2.GetPolygon()));

            //                if (intersect)
            //                {
            //                    region1.ConnectNeighbour(region2);
            //                }

            //            }
            //        }
            //    }
            //}
            context.SaveChanges();
        }


        private static void RegionCompute(Random random, JToken token, Contry contry)
        {
            bool first = true;
            foreach (JToken regionsToken in token["geometry"]["coordinates"])
            {
                var regionGeo = new List<Point>();
                if (regionsToken[0][0].Type is JTokenType.Float)
                {
                    foreach (JToken pointToken in regionsToken)
                    {
                        regionGeo.Add(new Point((float)pointToken[0], (float)pointToken[1]));
                    }
                    Point center = ComputeCenter(regionGeo);
                    var region = new Region(true, Convert.ToInt32(token["properties"]["pop_est"]), regionGeo, center);
                    contry.AddRegion(region);
                }
                else
                {
                    foreach (JToken regionToken in regionsToken)
                    {
                        foreach (JToken pointToken in regionToken)
                        {
                            regionGeo.Add(new Point((float)pointToken[0], (float)pointToken[1]));
                        }
                        Point center = ComputeCenter(regionGeo);
                        Region region = null;
                        if (first == true)
                        {
                            first = false;
                            region = new Region(true, Convert.ToInt32(token["properties"]["pop_est"]), regionGeo, center);
                        }
                        else
                        {
                            region = new Region(true, random.Next(1000), regionGeo, center);
                        }
                        contry.AddRegion(region);
                        regionGeo = new List<Point>();
                    }
                }
            }
        }


        public static Point ComputeCenter(List<Point> points)
        {
            {
                double totalX = 0;
                double totalY = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    totalX += points[i].X;
                    totalY += points[i].Y;
                }

                return new Point(totalX / (float)points.Count, totalY / (float)points.Count);
            }
        }




        public static bool PointCollectionsOverlap_Fast(PointCollection area1, PointCollection area2)
        {
            for (int i = 0; i < area1.Count; i++)
            {
                for (int j = 0; j < area2.Count; j++)
                {
                    if (LineSegmentsIntersect(area1[i], area1[(i + 1) % area1.Count], area2[j], area2[(j + 1) % area2.Count]))
                    {
                        return true;
                    }
                }
            }

            if (PointCollectionContainsPoint(area1, area2[0]) ||
                PointCollectionContainsPoint(area2, area1[0]))
            {
                return true;
            }

            return false;
        }

        public static bool PointCollectionContainsPoint(PointCollection area, Point point)
        {
            Point start = new Point(-100, -100);
            int intersections = 0;

            for (int i = 0; i < area.Count; i++)
            {
                if (LineSegmentsIntersect(area[i], area[(i + 1) % area.Count], start, point))
                {
                    intersections++;
                }
            }

            return (intersections % 2) == 1;
        }

        private static double Determinant(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.Y - vector1.Y * vector2.X;
        }

        private static bool LineSegmentsIntersect(Point _segment1_Start, Point _segment1_End, Point _segment2_Start, Point _segment2_End)
        {
            double det = Determinant(_segment1_End - _segment1_Start, _segment2_Start - _segment2_End);
            double t = Determinant(_segment2_Start - _segment1_Start, _segment2_Start - _segment2_End) / det;
            double u = Determinant(_segment1_End - _segment1_Start, _segment2_Start - _segment1_Start) / det;
            return (t >= 0) && (u >= 0) && (t <= 1) && (u <= 1);
        }


    }
}
