using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace GlobalBlueProject.Base
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ActionResult<Envelope<T>> Ok<T>(T result) =>
            base.Ok(Envelope<T>.Ok(result));

        protected ActionResult<Envelope<T>> Error<T>(string errorMessage) =>
            BadRequest(Envelope<T>.Error(errorMessage));

        protected ActionResult<Envelope<T>> OkFromResult<T>(Result<T> result) =>
            result.IsSuccess ? Ok(result.Value) : Error<T>(result.Error);
    }
}
