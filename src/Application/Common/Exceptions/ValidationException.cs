using FluentValidation.Results;

namespace TodoListApiCA.Application.Common.Exceptions;

/// <summary>
/// Đại diện cho các lỗi xảy ra trong quá trình xác thực (validation) Request/Command/Query.
/// Ngoại lệ này được sử dụng để gom nhóm và định dạng các lỗi từ FluentValidation thành một cấu trúc đồng nhất,
/// giúp Exception Handler Middleware ở tầng Web API có thể dễ dàng bắt và trả về phản hồi 400 Bad Request cho client.
/// </summary>
public class ValidationException : Exception
{
    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) 
        : this() // Gọi constructor mặc định để khởi tạo Errors và thông báo chung
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
