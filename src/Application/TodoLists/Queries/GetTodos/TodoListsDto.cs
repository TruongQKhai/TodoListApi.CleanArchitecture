using AutoMapper;
using Domain.Entities;

namespace TodoListApiCA.Application.TodoLists.Queries.GetTodos;

public class TodoListsDto
{
    public TodoListsDto()
    {
        Items = Array.Empty<TodoItemDto>();
    }

    public int Id { get; init; }

    public string? Title { get; init; }

    public string? Colour { get; init; }

    public IReadOnlyCollection<TodoItemDto> Items { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TodoList, TodoListsDto>();
        }
    }
}
