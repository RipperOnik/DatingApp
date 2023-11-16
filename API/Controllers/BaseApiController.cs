using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ServiceFilter(typeof(LogUserActivity))] // Update LastActive field on each request 
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{

}
