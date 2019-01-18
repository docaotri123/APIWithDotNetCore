using CityInfo.API.Entities;
using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public static class CityInfoExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if(context.Cities.Any())
            {
                return;
            }
            // init seed data
            var Cities = new List<City>()
            {
                new City()
                {
                    Name = "HCM",
                    Description = "HCM city is very large",
                    PointOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Central Park",
                            Description = "abc"
                        },
                         new PointOfInterest()
                        {
                            Name = "Landmark 81",
                            Description = "bdf"
                        }
                    }
                },
                 new City()
                {
                    Name = "Ha Noi",
                    Description = "Ha Noi city is very large",
                    PointOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Ho Guom lake",
                            Description = "abc"
                        },
                         new PointOfInterest()
                        {
                            Name = "Landmark 82",
                            Description = "bdf"
                        }
                    }
                }
            };

            context.Cities.AddRange(Cities);
            context.SaveChanges();
        }
    }
}
