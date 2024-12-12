using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

public class DeleteDishesCommand(int restaurantId) : IRequest
{
    public int RestaurantId { get; } = restaurantId;

}
