namespace TodoListApiCA.Application.Common.Security;

/// <summary>
/// Specifies the class this attribute is applied to required authorization.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    //public AuthorizeAttribute() { }

    /// <summary>
    /// Gets or sets a comma delimited list of roles that are allowed to access the resources.
    /// </summary>
    public string Roles { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the policy name that determines access to the resources.
    /// </summary>
    public string Policy { get; set; } = string.Empty;
}
