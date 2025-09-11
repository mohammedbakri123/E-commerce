using E_commerce_Endpoints.Shared;
using Microsoft.AspNetCore.Mvc;

public class MyControllerBase : ControllerBase

{
    protected IActionResult MapServiceResult<T>(ServiceResult<T> result)
    {
        return result.Error.Type switch
        {
            ServiceErrorType.None => Ok(result.Data),
            ServiceErrorType.Validation => BadRequest(result.Error.Message),
            ServiceErrorType.Duplicate => Conflict(result.Error.Message),
            ServiceErrorType.NotFound => NotFound(result.Error.Message),
            ServiceErrorType.ServerError => StatusCode(500, result.Error.Message),
            _ => StatusCode(500, "Unknown error")
        };
    }
}