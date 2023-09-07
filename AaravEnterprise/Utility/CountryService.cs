using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AaravEnterprise.Utility
{
    
    public class CountryService
    {
        private readonly ApplicationDbContext _dbContext;
        public CountryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Country> GetAllCountries()
        {
            return _dbContext.Countries.ToList();
        }
    }
}
