using API.Models.Requests.Accounts;
using Application.Features.Accounts.Commands;
using Application.Features.Accounts.Queries;
using Domain.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.V1
{
    [Authorize]
    [Route("api/v1/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        public AccountController(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var request = new GetBalanceRequest { AccountId = id };
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var result = await _mediator.Send(new GetListQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAccountRequest request)
        {
            var command = new CreateAccountCommand
            {
                AccountNumber = request.AccountNumber,
                AccountHolderName = request.AccountHolderName,
                Balance = request.Balance
            };
            var result = await _mediator.Send(command);
            if (result.IsSuccess())
            {
                await _publishEndpoint.Publish(new AccountCreated
                {
                    AccountId = result.Data
                });

            }


            return Ok(result);
        }

        [HttpPost("{id:guid}/deposit")]
        public async Task<IActionResult> DepositAsync(Guid id, [FromBody] DepositMoneyRequest request)
        {
            var command = new DepositMoneyCommand
            {
                AccountId = id,
                Amount = request.Amount
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id:guid}/withdraw")]
        public async Task<IActionResult> WithdrawAsync(Guid id, [FromBody] WithdrawMoneyRequest request)
        {
            var command = new WithdrawMoneyCommand
            {
                AccountId = id,
                Amount = request.Amount
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
