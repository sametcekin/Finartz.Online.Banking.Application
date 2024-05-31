using API.Models.Requests;
using Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand
        {
            Username = request.Username,
            Password = request.Password
        };
        var result = await _mediator.Send(command);
        return result.StatusCode == HttpStatusCode.BadRequest ? BadRequest(result) : Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] ReqisterRequest request)
    {
        var command = new RegisterCommand
        {
            Username = request.Username,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        var result = await _mediator.Send(command);
        return result.StatusCode == HttpStatusCode.BadRequest ? BadRequest(result) : Ok(result);
    }
}
