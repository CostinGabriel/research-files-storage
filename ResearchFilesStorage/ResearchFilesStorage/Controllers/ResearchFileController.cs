using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResearchFilesStorage.Application.Commands;
using ResearchFilesStorage.Application.Queries;
using ResearchFilesStorage.Domain.Entities;

namespace ResearchFilesStorage.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResearchFileController(IMediator mediator) : Controller
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ResearchFile>> GetResearchFile(Guid id, CancellationToken token)
      => await mediator.Send(new GetResearchFileByIdQuery(id), token);

    [HttpGet]
    public async Task<ActionResult<ResearchFile>> GetResearchFileByName([FromQuery] GetResearchFileByNameQuery request, CancellationToken token)
      => await mediator.Send(request, token);

    [HttpPost]
    public async Task<ActionResult<Guid>> AddResearchFile(AddResearchFileCommand request, CancellationToken token)
        => await mediator.Send(request, token);

    [HttpPost("addSecured")]
    public async Task<ActionResult<Guid>> AddSecuredResearchFile(AddSecuredResearchFileCommand request, CancellationToken token)
       => await mediator.Send(request, token);

    [HttpPut]
    public async Task UpdateResearchFile(UpdateResearchFileCommand request, CancellationToken token)
      => await mediator.Send(request, token);

    [HttpDelete("{id:guid}")]
    public async Task RemoveResearchFile(Guid id, CancellationToken token)
       => await mediator.Send(new RemoveResearchFileCommand(id), token);
}
