using System;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Basket.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class BasketController : ControllerBase
  {
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly EventRabbitMQProducer _eventBus;
    public BasketController(IBasketRepository basketRepository, IMapper mapper,
    EventRabbitMQProducer eventBus, ILogger<BasketController> logger)
    {
      _eventBus = eventBus;
      _logger = logger;
      _mapper = mapper;
      _basketRepository = basketRepository;
    }

  [HttpGet]
  public async Task<ActionResult<BasketCart>> GetBasket(string userName)
  {
    var basket = await _basketRepository.GetBasket(userName);
    if (string.IsNullOrEmpty(userName) || basket == null)
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

  [Route("[action]")]
  [HttpPost]
  public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
  {
    // get total price of the basket
    // remove the basket
    // send checkout event to rabbitMq

    var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
    if (basket == null)
    {
      _logger.LogError("Basket not exist with this user : {EventId}", basketCheckout.UserName);
      return BadRequest();
    }

    var basketRemoved = await _basketRepository.DeleteBasket(basketCheckout.UserName);
    if (!basketRemoved)
    {
      _logger.LogError("Basket can not deleted");
      return BadRequest();
    }

    // Once basket is checkout, sends an integration event to
    // ordering.api to convert basket to order and proceeds with
    // order creation process

    var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
    eventMessage.RequestId = Guid.NewGuid();
    eventMessage.TotalPrice = basket.TotalPrice;

    try
    {
      _eventBus.PublishBasketCheckout("BasketCheckoutQueue", eventMessage);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "ERROR Publishing integration event: {EventId} from {AppName}", eventMessage.RequestId, "Basket");
      throw;
    }

    return Accepted();
  }


}
}