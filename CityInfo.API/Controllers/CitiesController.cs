using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController: Controller
    {
        private ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            var cities = _cityInfoRepository.GetCities();
            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDTO>>(cities);

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePoints = true)
        {
            var city = _cityInfoRepository.GetCity(id, includePoints);
            if(city == null)
            {
                return NotFound();
            }
            if (includePoints)
            {
                //var cityResults = new CityDTO()
                //{
                //    ID = city.ID,
                //    Name = city.Name,
                //    Description = city.Description
                //};
                //foreach(var point in city.PointOfInterest)
                //{
                //    cityResults.PointOfInterest.Add(
                //        new PointOfInterestDTO()
                //        {
                //            ID = point.ID,
                //            Name = point.Name,
                //            Description = point.Description
                //        }
                //    );
                //}
                var cityResults = Mapper.Map<CityDTO>(city);
                return Ok(cityResults);
            }
            var cityWithoutPoints = Mapper.Map<CityWithoutPointsOfInterestDTO>(city);
            return Ok(cityWithoutPoints);
        }
    }
}
