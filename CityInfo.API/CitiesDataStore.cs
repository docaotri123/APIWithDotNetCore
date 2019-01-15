using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public List<CityDTO> Cities { get; set; }
        public CitiesDataStore()
        {
            Cities = new List<CityDTO>()
            {
                new CityDTO()
                {
                    ID = 1 ,
                    Name = "HCM",
                    Description = "HCM city is very large",
                    PointOfInterest = new List<PointOfInterestDTO>()
                    {
                        new PointOfInterestDTO()
                        {
                            ID=1,
                            Name = "Central Park",
                            Description = "abc"
                        },
                         new PointOfInterestDTO()
                        {
                            ID=2,
                            Name = "Landmark 81",
                            Description = "bdf"
                        }
                    }
                },
                 new CityDTO()
                {
                    ID = 1 ,
                    Name = "Ha Noi",
                    Description = "Ha Noi city is very large",
                    PointOfInterest = new List<PointOfInterestDTO>()
                    {
                        new PointOfInterestDTO()
                        {
                            ID=1,
                            Name = "Ho Guom lake",
                            Description = "abc"
                        },
                         new PointOfInterestDTO()
                        {
                            ID=2,
                            Name = "Landmark 82",
                            Description = "bdf"
                        }
                    }
                }
            };
        }
    }
}
