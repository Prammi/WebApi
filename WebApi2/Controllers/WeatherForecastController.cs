using Contracts;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2.Controllers
{
    [ApiController]
    [Route("[controller]")]
  
    public class WeatherForecastController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        public WeatherForecastController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var domesticAccounts = _repoWrapper.Account.FindByCondition(x => x.AccountType.Equals("Domestic"));
            var owners = _repoWrapper.Owner.FindAll();
            return new string[] { "value1", "value2" };
        }
    }
}
