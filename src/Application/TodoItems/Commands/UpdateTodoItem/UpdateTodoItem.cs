using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace TodoListApiCA.Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand : IRequest
{
    public int Id { get; init; }
    public string? Title { get; init; }
    public bool Done { get; init; }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand>
{
    private readonly IAppDbContext _context;

    public UpdateTodoItemCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems.FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;
        entity.Done = request.Done;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
