using System.Threading.Tasks;
using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class BasketController : ControllerBase
  {
    private readonly IBasketRepository _basketRepository;
    public BasketController(IBasketRepository basketRepository)
    {
      _basketRepository = basketRepository;
    }

    [HttpGet]
    public async Task<ActionResult<BasketCart>> GetBasket(string userName)
    {
        var basket = await _basketRepository.GetBasket(userName);
        if(string.IsNullOrEmpty(userName) || basket == null)
            return BadRequest("User or basket not found.");
        return Ok(basket);
    }

    [HttpPost]
    public async Task<ActionResult<BasketCart>> UpdateBasket([FromBody] BasketCart basket)
    {
        return Ok(await _basketRepository.UpdateBasket(basket));
    }

    [HttpDelete("{userName}")]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        return Ok(await _basketRepository.DeleteBasket(userName));
    }

  }
}