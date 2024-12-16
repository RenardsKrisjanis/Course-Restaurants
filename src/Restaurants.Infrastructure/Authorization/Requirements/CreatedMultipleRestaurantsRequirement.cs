using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class CreatedMultipleRestaurantsRequirement(int minimumRestaurants) : IAuthorizationRequirement
{
    public int MinimumRestaurantsCreated { get; } = minimumRestaurants;

}
