using ExchangeRateApp.Services;
using ExchangeRateApp;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExchangeRateApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public ExchangeRateController(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetExchangeRates([FromQuery] List<string> currencyCodes)
        {
            try
            {
                var currencies = new List<Currency>();
                foreach (var code in currencyCodes)
                {
                    currencies.Add(new Currency(code));
                }

                var result = await _exchangeRateProvider.GetExchangeRates(currencies);

                if (result.Count() == 0 || result is null)
                    return NotFound("Currency not found");

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
