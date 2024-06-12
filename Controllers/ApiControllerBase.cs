using Microsoft.AspNetCore.Mvc;

namespace BooksAPI.Controllers;
[ApiController]
[Route(   BaseApiPath+"[controller]")]
public class ApiControllerBase:ControllerBase
{
    private const string BaseApiPath = "/api";
}