using System;
using System.Linq;
using System.Timers;
using LpApi_20210506.Common;
using LpApi_20210506.DataAccess;
using LpApi_20210506.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LpApi_20210506.Main.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class CustomersApiController : ControllerBase
    {
        #region Private Properties And Ctor

        private readonly ILogger<CustomersApiController> _logger;
        private readonly IDatabaseRepository _databaseRepository;
        private readonly GrandParadeConfiguration _grandParadeConfiguration;
        
        private static int _getCallsCount;
        private static readonly object GetCustomersLock = new object();
        private static Timer _getCallsTimer;

        public CustomersApiController(
            ILogger<CustomersApiController> logger,
            IDatabaseRepository databaseRepository,
            IOptions<GrandParadeConfiguration> config)
        {
            _databaseRepository = databaseRepository;
            _logger = logger;
            _grandParadeConfiguration = config.Value;
            if (ReferenceEquals(_getCallsTimer, null))
            {
                _getCallsTimer = new Timer()
                {
                    Interval = _grandParadeConfiguration.MillisecondsToResetGetCounter
                };
                _getCallsTimer.Elapsed += LimitingTimerElapsed;
                _getCallsTimer.Start();
            }
        }

        #endregion Private Properties And Ctor

        #region Api Methods

        [HttpPost("~registercustomer")]
        public IActionResult RegisterCustomer(string firstName,
            string lastName, 
            string street,
            string houseNumber,
            string zipCode,
            string personalNumber,
            string favoriteFootballTeam,
            CustomerSourceEnum customerSource)
        {
            var customer = new Customer(firstName, lastName, houseNumber, street, zipCode, personalNumber, favoriteFootballTeam, customerSource);

            try
            {
                var validateModel = customer.ValidateModel();
                if (validateModel.Count > 0)
                {
                    var message = string.Format(SharedResource.DataIsNotValid,
                        string.Join(' ', validateModel.Select(x => x.ErrorMessage).ToArray()));
                    _logger.LogError(message);
                    return BadRequest(message);
                }

                if (_databaseRepository.GetCustomer(customer.MainKey) == null)
                {
                    var saved = _databaseRepository.SaveCustomer(customer);
                    if (saved)
                    {
                        _logger.LogInformation("Successfully saved customer with firstName: {0}, lastName: {1}", firstName, lastName);
                        return Ok(customer);
                    }
                }

                _logger.LogInformation("Did not save customer with firstName: {0}, lastName: {1}", firstName, lastName);
                return BadRequest(SharedResource.CustomerAlreadyExists);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("~getcustomers")]
        public IActionResult GetCustomers()
        {
            lock (GetCustomersLock)
            {
                _getCallsCount++;
                if (_getCallsCount > _grandParadeConfiguration.MaximumCallsInCounter)
                {
                    _logger.LogInformation("Too many calls to GetCustomers API method");
                    return BadRequest(SharedResource.TooManyCalls);
                }

                var customersList = _databaseRepository.GetCustomers();
                _logger.LogInformation("Retrieved {0} customers and returned via GET call", customersList.Count);

                return Ok(customersList);
            }
        }

        #endregion Api Methods

        #region Private Methods

        private void LimitingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _getCallsCount = 0;
        }

        #endregion Private Methods
    }
}
