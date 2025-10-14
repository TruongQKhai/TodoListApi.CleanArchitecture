using TodoListApiCA.Application.Common.Models;

namespace TodoListApiCA.Application.TodoLists.Queries.GetTodos;

public class TodosVm
{
    public IReadOnlyCollection<LookupDto> PriorityLevels { get; init; } = Array.Empty<LookupDto>();
    public IReadOnlyCollection<TodoListsDto> Lists { get; init; } = Array.Empty<TodoListsDto>();
}
