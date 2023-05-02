using ApiTemplate.Application.Common;
using ApiTemplate.Application.Samples;
using ApiTemplate.Application.Samples.Commands.Add;
using ApiTemplate.Application.Samples.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.API.Controllers.Samples;

[ApiController]
[Route("samples")]
public class SamplesController : ControllerBase
{
    private readonly ILogger<SamplesController> _logger;
    private readonly IMediator _mediator;

    public SamplesController(ILogger<SamplesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [SwaggerOperation(
        Summary = "Add a sample",
        Description = "Add a sample",
        OperationId = "Samples_Add",
        Tags = new[] { "Samples" })]
    [SwaggerResponse(200, "Sample added", typeof(Guid))]
    [SwaggerResponse(400, "Bad request", typeof(IEnumerable<string>))]
    [HttpPost]
    public async Task<ActionResult<Guid>> Add([FromBody] AddSampleCommand command)
    {
        return await _mediator.SendAndReturnObjectActionResult(command);
    }
    
    [SwaggerOperation(
        Summary = "Get all samples",
        Description = "Get all samples",
        OperationId = "Samples_GetAll",
        Tags = new[] { "Samples" })]
    [SwaggerResponse(200, "Samples", typeof(IEnumerable<SampleDto>))]
    [SwaggerResponse(400, "Bad request", typeof(IEnumerable<string>))]
    [HttpGet]
    public async Task<ActionResult<PaginatedList<SampleDto>>> GetAll([FromQuery] GetAllSamplesQuery query)
    {
        return await _mediator.SendAndReturnObjectActionResult(query);
    }
}