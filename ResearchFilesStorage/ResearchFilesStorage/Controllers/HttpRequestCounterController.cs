using Microsoft.AspNetCore.Mvc;
using ResearchFilesStorage.Infrastructure;

namespace ResearchFilesStorage.Controllers;


[ApiController]
[Route("[controller]")]
public class HttpRequestCounterController : ControllerBase
{

    [HttpGet()]
    public long Get() => HttpRequestCounter.Instance.GetCount();
}
