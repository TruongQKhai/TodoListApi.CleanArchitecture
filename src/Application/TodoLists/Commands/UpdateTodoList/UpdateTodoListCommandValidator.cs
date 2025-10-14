using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace TodoListApiCA.Application.TodoLists.Commands.UpdateTodoList;

public class UpdateTodoListCommandValidator : AbstractValidator<UpdateTodoListCommand>
{
    private readonly IAppDbContext _context;

    public UpdateTodoListCommandValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueTitle)
                .WithMessage("'PropertyName' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueTitle(UpdateTodoListCommand model, string title, CancellationToken cancellationToken)
    {
        return !await _context.TodoLists
            .Where(x => x.Id != model.Id)
            .AnyAsync(x => x.Title == title, cancellationToken);
    }
}
