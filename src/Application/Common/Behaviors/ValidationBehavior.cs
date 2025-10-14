using FluentValidation;
using MediatR;
using ValidationException = TodoListApiCA.Application.Common.Exceptions.ValidationException;

namespace TodoListApiCA.Application.Common.Behaviors;

/// <summary>
/// đóng vai trò là một Interceptor (bộ chặn) trong pipeline xử lý request/response của MediatR.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : notnull // Đảm bảo kiểu Request (Command/Query) không được là null
{
    // Danh sách tất cả các Validator (FluentValidation IValidator) được đăng ký cho kiểu TRequest hiện tại
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Kiểm tra xem có bất kỳ Validator nào được tìm thấy cho Request này không
        if (_validators.Any())
        {
            // Chạy tất cả các Validators tìm thấy một cách bất đồng bộ (Task.WhenAll)
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(new ValidationContext<TRequest>(request), cancellationToken)));

            // Tổng hợp tất cả các lỗi xác thực (ValidationFailure) từ tất cả các Validators
            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);
        }

        // Nếu không có lỗi, tiếp tục pipeline, cho phép yêu cầu đi đến Handler (logic nghiệp vụ)
        return await next();
    }
}
