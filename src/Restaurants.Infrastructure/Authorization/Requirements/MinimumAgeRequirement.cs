using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumAgeRequirement(int minimumWage) : IAuthorizationRequirement
{
    public int MinimumAge { get; } = minimumWage;
    
}
