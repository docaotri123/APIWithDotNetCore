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

        public PointOfInterestController(ILogger<PointOfInterestController> logger, LocalMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
          //  HttpContext.RequestServices.GetService()

        }

        [HttpGet("{idCity}/pointofinterest")]
        public IActionResult GetPointOfInterest(int idCity)
        {
            var city = (CitiesDataStore.Current.Cities.FirstOrDefault(m => m.ID.Equals(idCity)));
            if(city==null)
            {
                _logger.LogInformation($"City with id {idCity} was not found");
                return NotFound();
            }
            return Ok(city.PointOfInterest);
        }

        [HttpGet("{idCity}/pointofinterest/{idInterest}", Name ="GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int idCity,int idInterest)
        {
            var city = (CitiesDataStore.Current.Cities.FirstOrDefault(m => m.ID.Equals(idCity)));
            if (city == null)
            {
                return NotFound();
            }
            var interest = city.PointOfInterest.FirstOrDefault(m => m.ID == idInterest);
            if (interest == null)
            {
                return NotFound();
            }
            return Ok(interest);
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
            var city = (CitiesDataStore.Current.Cities.FirstOrDefault(m => m.ID.Equals(idCity)));
            if (city == null)
            {
                return NotFound();
            }
            var maxPoint = city.PointOfInterest.Max(p => p.ID);

            var createPoint = new PointOfInterestDTO()
            {
                ID = ++maxPoint,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointOfInterest.Add(createPoint);
            return CreatedAtRoute("GetPointOfInterest",new { cityId =idCity , id=createPoint.ID}, createPoint);
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
