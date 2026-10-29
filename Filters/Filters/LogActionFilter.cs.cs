using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace ProductManagement.Filters.Filters
{
    public class LogActionFilter : IActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;
        private Stopwatch _sw = new();
        public LogActionFilter(ILogger<LogActionFilter> logger) => _logger = logger;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _sw = Stopwatch.StartNew();
            _logger.LogInformation("Starting: {Action}", context.ActionDescriptor.DisplayName);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _sw.Stop();
            _logger.LogInformation("Finished in {Ms} ms", _sw.ElapsedMilliseconds);
        }
    }
}
