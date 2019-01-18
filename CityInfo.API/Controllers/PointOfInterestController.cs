using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestController:Controller
    {
        private ILogger<PointOfInterestController> _logger;
        private LocalMailService _mailService;
        private ICityInfoRepository _cityInfoRepository;

        public PointOfInterestController(ILogger<PointOfInterestController> logger,
            LocalMailService mailService,
            ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
          //  HttpContext.RequestServices.GetService()

        }

        [HttpGet("{idCity}/pointofinterest")]
        public IActionResult GetPointOfInterest(int idCity)
        {
            var city = _cityInfoRepository.GetCity(idCity, true);
            if(city==null)
            {
                _logger.LogInformation($"City with id {idCity} was not found");
                return NotFound();
            }

            var points = Mapper.Map<IEnumerable<PointOfInterestDTO>>(city.PointOfInterest);
            return Ok(points);
        }

        [HttpGet("{idCity}/pointofinterest/{idInterest}", Name ="GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int idCity,int idInterest)
        {
            var city = _cityInfoRepository.GetCity(idCity, true);
            if (city == null)
            {
                _logger.LogInformation($"City with id {idCity} was not found");
                return NotFound();
            }

            var point = city.PointOfInterest.Where(m => m.ID == idInterest).FirstOrDefault();
            if(point == null)
            {
                return NotFound();
            }
            var pointResult = Mapper.Map<PointOfInterestDTO>(point);
            return Ok(pointResult);
        }
        [HttpPost("{idCity}/pointofinterest")]
        public IActionResult CreatePointOfInterest(int idCity,[FromBody] PointOfInterestForCreationDTO pointOfInterest)
        {
            if(pointOfInterest == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var city = _cityInfoRepository.GetCity(idCity, true);
            if (!_cityInfoRepository.CityExists(idCity))
            {
                return NotFound();
            }

            var point = Mapper.Map<PointOfInterest>(pointOfInterest);
            point.CityId = idCity;

            var checkSave = _cityInfoRepository.AddPointIntoCity(idCity, point);
            if (!checkSave)
            {
                return StatusCode(500, "A problem happened while handling your request");
            }
            return CreatedAtRoute("GetPointOfInterest",new { cityId =idCity , id=point.ID}, point);
        }

        [HttpPut("{idCity}/pointofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int idCity,int id, [FromBody] PointOfInterestForUpdateDTO pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var city = (CitiesDataStore.Current.Cities.FirstOrDefault(m => m.ID.Equals(idCity)));
            if (city == null)
            {
                return NotFound();
            }
            var point = city.PointOfInterest.FirstOrDefault(p=> p.ID.Equals(id));

            if(point == null)
            {
                return BadRequest();
            }

            point.Name = pointOfInterest.Name;
            point.Description = pointOfInterest.Description;
            return NoContent();

        }

        [HttpPatch("{idCity}/pointofinterest/{id}")]
        public IActionResult PartialUpdatePointOfInterest(int idCity, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            var city = (CitiesDataStore.Current.Cities.FirstOrDefault(m => m.ID.Equals(idCity)));
            if (city == null)
            {
                return NotFound();
            }
            var point = city.PointOfInterest.FirstOrDefault(p => p.ID.Equals(id));

            if (point == null)
            {
                return BadRequest();
            }
            var updatePoint = new PointOfInterestForUpdateDTO()
            {
                Name = point.Name,
                Description = point.Description
            };

            patchDoc.ApplyTo(updatePoint, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }


            point.Name = updatePoint.Name;
            point.Description = updatePoint.Description;
            return NoContent();

        }

        [HttpDelete("{idCity}/pointofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int idCity, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(m=> m.ID.Equals(idCity));
            if(city == null)
            {
                return BadRequest();
            }
            var point = city.PointOfInterest.FirstOrDefault(c => c.ID.Equals(id));
            if (point == null)
            {
                return BadRequest();
            }
            city.PointOfInterest.Remove(point);
            _mailService.Send("point", $"{point.Name} with {point.ID} was deleted");
            return NoContent();
        }
    }
}
