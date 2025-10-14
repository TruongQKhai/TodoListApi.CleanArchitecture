using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// A TodoList can have multiple TodoItem
/// </summary>
public class TodoList : BaseAuditableEntity
{
    public string? Title { get; set; }
    public Colour Colour { get; set; } = Colour.White;

    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
}
