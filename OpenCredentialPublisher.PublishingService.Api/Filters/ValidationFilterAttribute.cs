using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace OpenCredentialPublisher.PublishingService.Api
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.ErrorCount > 0)
            {
                //var modelType = context.ActionDescriptor.Parameters
                //    .FirstOrDefault(p => p.BindingInfo.BindingSource.Id.Equals("Body", StringComparison.InvariantCultureIgnoreCase))?.ParameterType;

                var errors = context.ModelState
                    .Where(v => v.Value.ValidationState == ModelValidationState.Invalid)
                    .SelectMany(v => v.Value.Errors.Select(e => $"[{v.Key}] {e.ErrorMessage}"))
                    .ToList();

                context.Result = new BadRequestObjectResult(new { Error = true, Errors = errors });
            }
            base.OnActionExecuting(context);
        }
    }

}
