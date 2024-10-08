using Fiap.TechChallenge.Application.Services.Interfaces;
using Fiap.TechChallenge.Domain.Entities;
using Fiap.TechChallenge.Domain.Request;
using Fiap.TechChallenge.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.TechChallenge.Api.Controllers;

[Route("api/[Controller]")]
[ApiController]
[AllowAnonymous]
public class ContactController(IContactService contactService, ILogger<ContactController> logger) : Controller
{
    /// <summary>
    /// List all contacts
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Contacts</returns>
    /// <response code="200">OK</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await contactService.GetAllAsync(cancellationToken);
        return Ok(result);
    }
    
    /// <summary>
    /// Find contact by id
    /// </summary>
    /// <param name="id">Unique contact identify</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Contact</returns>
    /// <response code="200">OK</response>
    /// <response code="204">No content</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById([FromRoute]long id, CancellationToken cancellationToken)
    {
        var result = await contactService.GetByIdAsync(id, cancellationToken);
        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }
    
    /// <summary>
    /// Find contacts by DDD
    /// </summary>
    /// <param name="dddNumber">Contact DDD</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Contacts</returns>
    /// <response code="200">OK</response>
    /// <response code="204">No content</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("ddd/{dddNumber}")]
    public async Task<IActionResult> GetByDdd([FromRoute]short dddNumber, CancellationToken cancellationToken)
    {
        var result = await contactService.GetAllByDddAsync(dddNumber, cancellationToken);
        if (!result.Any())
        {
            return NoContent();
        }
        return Ok(result);
    }
    
    /// <summary>
    /// Create a new contact
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="201">Created</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]ContactPostRequest request, CancellationToken cancellationToken)
    {
        if (await contactService.CreateAsync(request, cancellationToken))
        {
            return Accepted();
        }
        
        return BadRequest(new DefaultResponse<ContactPostRequest> { Message = "Contact not created.", Data = request});
    }
    
    /// <summary>
    /// Update contact by Id
    /// </summary>
    /// <param name="id">Id of contact</param>
    /// <param name="request">Contact Request payload with changes</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Contact</returns>
    /// <response code="200">OK</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Internal server error</response>
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update([FromRoute] long id, ContactPutRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;
        
        if (await contactService.UpdateAsync(request, cancellationToken))
        {
            return Accepted();
        }
        
        return BadRequest(new DefaultResponse<ContactPutRequest> { Message = "Contact not updated.", Data = request });
    }
    
    /// <summary>
    /// Delete contact by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">OK</response>
    /// <response code="400">Bad request</response>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete([FromRoute]long id, CancellationToken cancellationToken)
    {
        if (await contactService.DeleteAsync(id, cancellationToken))
        {
            return Accepted();
        }
        
        return BadRequest(new DefaultResponse<Contact> { Message = "Contact not deleted."});
    }
}