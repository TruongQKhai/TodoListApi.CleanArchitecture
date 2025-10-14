using MediatR;
using Microsoft.Extensions.Logging;

namespace TodoListApiCA.Application.Common.Behaviors;

/// <summary>
/// Class này là một 'Behavior' trong Pipeline của MediatR, dùng để bắt và ghi log tất cả các lỗi chưa được xử lý (Unhandled Exceptions).
/// </summary>
public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(ex, "TodoListApi Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}
