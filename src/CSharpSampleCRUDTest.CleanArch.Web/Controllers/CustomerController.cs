using System.Diagnostics;
using AutoMapper;
using CSharpSampleCRUDTest.CleanArch.Core.CustomerAggregate.Entities;
using CSharpSampleCRUDTest.CleanArch.UseCases.Customers;
using CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Create;
using CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Delete;
using CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Get;
using CSharpSampleCRUDTest.CleanArch.UseCases.Customers.List;
using CSharpSampleCRUDTest.CleanArch.UseCases.Customers.Update;
using CSharpSampleCRUDTest.CleanArch.Web.Configurations;
using CSharpSampleCRUDTest.CleanArch.Web.MapperProfiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSharpSampleCRUDTest.CleanArch.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly Mapper _updateApiMapper;
  private readonly Mapper _apiMapper;
  private readonly ActivitySource _activitySource;

  public CustomerController(IMediator mediator, Instrumentation instrumentation)
  {
    _mediator = mediator;
    _activitySource = instrumentation.ActivitySource;

    var configUpdateApiMapper = new MapperConfiguration(cfg =>
    {
      cfg.AddProfile(new UpdateCustomerDtoANDCustomerMapperProfile());
    });
    _updateApiMapper = new Mapper(configUpdateApiMapper);

    var configApiMapper = new MapperConfiguration(cfg =>
    {
      cfg.AddProfile(new CustomerDtoANDCustomerMapperProfile());
    });
    _apiMapper = new Mapper(configApiMapper);

    using var myActivity = _activitySource.StartActivity();
  }

  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<UpdateCustomerDTO>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetAll()
  {
    var result = await _mediator.Send(new GetCustomerListQuery());
    if (result is null || !result.Any())
      return StatusCode(StatusCodes.Status204NoContent);

    var resultMapped = _updateApiMapper.Map<IEnumerable<UpdateCustomerDTO>>(result);

    return Ok(resultMapped);
  }

  [HttpGet("{id:Guid}")]
  [ProducesResponseType(typeof(UpdateCustomerDTO), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetById([FromRoute] Guid id)
  {
    var result = await _mediator.Send(new GetCustomerQuery(id));

    var resultMapped = _updateApiMapper.Map<UpdateCustomerDTO>(result);

    return Ok(resultMapped);
  }

  [HttpPost]
  [ProducesResponseType(typeof(UpdateCustomerDTO), StatusCodes.Status201Created)]
  public async Task<IActionResult> Add([FromBody] CustomerDTO model)
  {

    var result = await _mediator.Send(new CreateCustomerCommand(_apiMapper.Map<Customer>(model)));

    var resultMapped = _updateApiMapper.Map<UpdateCustomerDTO>(result);

    return Created("~/", resultMapped);
  }

  [HttpPut]
  [ProducesResponseType(typeof(UpdateCustomerDTO), StatusCodes.Status200OK)]
  public async Task<IActionResult> Update([FromBody] UpdateCustomerDTO model)
  {

    var result = await _mediator.Send(new UpdateCustomerCommand(_updateApiMapper.Map<Customer>(model)));

    var resultMapped = _updateApiMapper.Map<UpdateCustomerDTO>(result);

    return Ok(resultMapped);
  }

  [HttpDelete("{id:Guid}")]
  [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
  public async Task<IActionResult> Delete([FromRoute] Guid id)
  {

    await _mediator.Send(new DeleteCustomerCommand(id));

    return NoContent();
  }
}
