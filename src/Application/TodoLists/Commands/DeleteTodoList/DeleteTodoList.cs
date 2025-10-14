using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoListApiCA.Application.Common.Security;
using TodoListApiCA.Domain.Constants;

namespace TodoListApiCA.Application.TodoLists.Commands.DeleteTodoList;

[Authorize(Roles = Roles.Administrator)]
[Authorize(Policy = Policies.CanDeleteTodo)]
public record DeleteTodoListCommand(int Id) : IRequest;

public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand>
{
    private readonly IAppDbContext _context;

    public DeleteTodoListCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists
            .Where(x => x.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.TodoLists.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}