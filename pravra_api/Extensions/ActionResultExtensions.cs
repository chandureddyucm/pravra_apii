using Microsoft.AspNetCore.Mvc;
using pravra_api.Models;

namespace pravra_api.Extensions
{
    public static class ActionResultExtensions
    {
        public static IActionResult ToActionResult<T>(this ServiceResponse<T> response)
        {
            if (response.Status)
                return new OkObjectResult(response); // Returns 200 OK with response data
            else
                return new BadRequestObjectResult(response); // Returns 400 Bad Request with response data
        }
    }
}