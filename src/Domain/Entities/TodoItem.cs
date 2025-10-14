using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// A TodoItem belong to only one TodoList
/// </summary>
public class TodoItem : BaseAuditableEntity
{
    public int ListId { get; set; }

    public string? Title { get; set; }

    public string? Note { get; set; }

    public PriorityLevel Priority { get; set; }

    public DateTime? Reminder { get; set; }

    private bool _done;

    public bool Done
    {
        get => _done;
        set
        {
            if (value && !_done)
            {
                // add domain event
            }

            _done = value;
        }
    }

    public TodoList List { get; set; } = null!;
}
