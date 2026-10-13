using App.Common.Security;

namespace App.Clusters.Queries.GetClusterAdminForDisplay;

[Authorize]
public record GetClusterAdminForDisplayQuery(string Id) : IRequest<ClusterAdminDisplayDto?>;
