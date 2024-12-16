using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowedPageSizes = [5, 10, 15, 30];
    private string[] allowedSortByColumnNames = 
        [nameof(RestaurantDto.Name),
        nameof(RestaurantDto.Description),
        nameof(RestaurantDto.Category)];
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize)
            .Must(x => allowedPageSizes.Contains(x))
            .WithMessage($"PageSize must be one of the following: {string.Join(", ", allowedPageSizes)}");

        RuleFor(x => x.SortBy)
            .Must(x => allowedSortByColumnNames.Contains(x))
            .When(x => x.SortBy != null)
            .WithMessage($"PageSize must be one of the following: {string.Join(", ", allowedSortByColumnNames)}");
    }
}