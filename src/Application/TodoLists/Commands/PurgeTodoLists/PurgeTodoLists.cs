using Application.Common.Interfaces;
using MediatR;

namespace TodoListApiCA.Application.TodoLists.Commands.PurgeTodoLists;

public record PurgeTodoListsCommand : IRequest;


public class PurgeTodoListsCommandHandler : IRequestHandler<PurgeTodoListsCommand>
{
    private readonly IAppDbContext _context;

    public PurgeTodoListsCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(PurgeTodoListsCommand request, CancellationToken cancellationToken)
    {
        _context.TodoLists.RemoveRange(_context.TodoLists);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

