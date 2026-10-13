using App.Common.Security;
using Domain.Constants;

namespace App.Clusters.Commands.DeleteClusterAdmin;

[Authorize(Roles = Roles.Administrator)]
public record DeleteClusterAdminCommand(string Id) : IRequest<bool>;
