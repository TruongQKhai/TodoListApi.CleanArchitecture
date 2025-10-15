using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TodoListApiCA.Application.Common.Models;
using TodoListApiCA.Application.TodoItems.Commands.CreateTodoItem;
using TodoListApiCA.Application.TodoItems.Commands.UpdateTodoItem;
using TodoListApiCA.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using WebAPI.Infrustructure;

namespace TodoListApiCA.WebAPI.Endpoints;

public class TodoItems : EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder) // groupBuilder contains _prefix = '/api/{groupName}' = /api/TodoLists
    {
        groupBuilder.MapGet(GetTodoItemsWithPagination).RequireAuthorization();
        groupBuilder.MapPost(CreateTodoItem).RequireAuthorization();
        groupBuilder.MapPut(UpdateTodoItem, "{id}").RequireAuthorization();
    }

    public async Task<Created<int>> CreateTodoItem(ISender sender, CreateTodoItemCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(TodoItems)}/{id}", id);
    }

    public async Task<Results<NoContent, BadRequest>> UpdateTodoItem(ISender sender, int id, UpdateTodoItemCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public async Task<Ok<PaginatedList<TodoItemBriefDto>>> GetTodoItemsWithPagination(
        ISender sender,
        [AsParameters] GetTodoItemsWithPaginationQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }
}
