using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using TodoListApiCA.Application.Common.Behaviors;
using TodoListApiCA.Application.Common.Interfaces;
using TodoListApiCA.Application.TodoItems.Commands.CreateTodoItem;

namespace TodoListApiCA.Application.UnitTests.Common.Behaviors;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateTodoItemCommand>> _logger = null!;
    private Mock<IUser> _user = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateTodoItemCommand>>();
        _user = new Mock<IUser>();
        _identityService = new Mock<IIdentityService>();
    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        _user.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());

        var requestLogger = new LoggingBehavior<CreateTodoItemCommand>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "tittle" }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        var requestLogger = new LoggingBehavior<CreateTodoItemCommand>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "tittle" }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
    }
}
