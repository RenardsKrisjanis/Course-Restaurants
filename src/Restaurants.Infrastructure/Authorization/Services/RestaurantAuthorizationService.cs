using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
    IUserContext userContext) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();

        logger.LogInformation("Checking if user {UserId} is authorized to {ResourceOperation} restaurant {RestaurantId}",
            user!.UserId, resourceOperation, restaurant.Id);

        if(resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
        {
            logger.LogInformation("Authorization succeeded");
            return true;
        }

        if (resourceOperation == ResourceOperation.Delete && user.IsinRole(UserRoles.Admin))
        {
            logger.LogInformation("Authorization succeeded");
            return true;
        }

        if ((resourceOperation == ResourceOperation.Update || resourceOperation == ResourceOperation.Delete) && user.UserId == restaurant.OwnerId)
        {
            logger.LogInformation("Authorization succeeded");
            return true;
        }

        logger.LogWarning("Authorization failed");
        return false;
    }
}
