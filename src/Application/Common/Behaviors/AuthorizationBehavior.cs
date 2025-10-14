using MediatR;
using System.Reflection;
using TodoListApiCA.Application.Common.Exceptions;
using TodoListApiCA.Application.Common.Interfaces;
using TodoListApiCA.Application.Common.Security;

namespace TodoListApiCA.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public AuthorizationBehavior(
        IUser user,
        IIdentityService identityService)
    {
        _user = user;
        _identityService = identityService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any())
        {
            // Must be authenticated user
            if (_user.Id == null)
                throw new UnauthorizedAccessException();

            // Role-based authorization
            var authorizeAttributesWithRoles = authorizeAttributes
                .Where(a => !string.IsNullOrWhiteSpace(a.Roles))
                .Select(a => a.Roles.Split(','));

            if (authorizeAttributesWithRoles.Any())
            {
                var authorized = false;

                foreach (var roles in authorizeAttributesWithRoles)
                {
                    foreach (var role in roles)
                    {
                        var isInRole = _user.Roles?.Any(r => r == role) ?? false;
                        if (isInRole)
                        {
                            authorized = true;
                            break;
                        }
                    }
                }

                // Must be a member of at least one role in roles
                if (!authorized)
                    throw new ForbiddenAccessException();
            }

            // Policy-based authorization
            var authorizeAttributesWithPolicies = authorizeAttributes
                .Where(a => !string.IsNullOrWhiteSpace(a.Policy))
                .Select(a => a.Policy);

            foreach (var policy in authorizeAttributesWithPolicies)
            {
                var authorized = await _identityService.AuthorizeAsync(_user.Id, policy);

                if (!authorized)
                    throw new ForbiddenAccessException();
            }
        }

        // User is authorized/ authorization not required
        return await next();
    }
}
