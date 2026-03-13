using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using Microsoft.AspNetCore.Http;

namespace questionmark.Api.Filters
{
    public class ValidationBehaviour : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            var result = new BadRequestObjectResult(new { ValidationErrors = validationProblemDetails?.Errors });
            result.StatusCode = 400; //Ovo je 400 svakako ali ne škodi da ga opet postavim.
            return result;
        }
    }
}
