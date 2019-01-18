using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        public bool AddPointIntoCity(int cityId, PointOfInterest point)
        {
            var city = GetCity(cityId, true);
            if (city == null)
                return false;
            city.PointOfInterest.Add(point);
            _context.SaveChanges();

            return true;
        }

        public bool CityExists(int cityId)
        {
            var city = _context.Cities.Where(m => m.ID == cityId).FirstOrDefault();
            return city != null;
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities
                .OrderBy(e => e.Name)
                .ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return _context.Cities
                    .Include(x => x.PointOfInterest)
                    .Where(m => m.ID == cityId)
                    .FirstOrDefault();

            }
            return _context.Cities
                .Where(m => m.ID == cityId)
                .FirstOrDefault();
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointId)
        {
            var city = GetCity(cityId, true);
            if (city == null)
                return null;
            return city.PointOfInterest
                .FirstOrDefault(m => m.ID == pointId);
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            var city = GetCity(cityId, true);
            if (city == null)
                return null;
            return city.PointOfInterest.ToList();
        }
    }
}
