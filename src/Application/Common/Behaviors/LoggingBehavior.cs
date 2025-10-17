using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using TodoListApiCA.Application.Common.Interfaces;

namespace TodoListApiCA.Application.Common.Behaviors;

public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public LoggingBehavior(ILogger<TRequest> logger, IUser user, IIdentityService identityService)
    {
        _logger = logger;
        _user = user;
        _identityService = identityService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _user.Id ?? string.Empty;
        var userName = string.Empty;

        if (!string.IsNullOrEmpty(userId))
            userName = await _identityService.GetUserNameAsync(userId);

        _logger.LogInformation(
            "TodoListApi Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);
    }
}
