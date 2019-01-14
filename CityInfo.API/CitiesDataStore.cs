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
                    Description = "HCM city is very large"
                },
                 new CityDTO()
                {
                    ID = 1 ,
                    Name = "Ha Noi",
                    Description = "Ha Noi city is very large"
                }
            };
        }
    }
}
