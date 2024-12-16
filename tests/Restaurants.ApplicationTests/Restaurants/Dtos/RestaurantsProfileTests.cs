using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Restaurants.Dtos.Tests;

public class RestaurantsProfileTests
{
    private IMapper _mapper;

    public RestaurantsProfileTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<RestaurantsProfile>());
        _mapper = configuration.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        // Arrange
        
        var restaurant = new Restaurant
        {
            Id = 1,
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Test Category",
            HasDelivery = true,
            Address = new Address
            {
                City = "Test City",
                Street = "Test Street",
                PostalCode = "Test Postal Code"
            }
        };

        // Act

        var result = _mapper.Map<RestaurantDto>(restaurant);

        // Assert

        result.Should().NotBeNull();
        result.Id.Should().Be(restaurant.Id);
        result.Name.Should().Be(restaurant.Name);
        result.Description.Should().Be(restaurant.Description);
        result.Category.Should().Be(restaurant.Category);
        result.HasDelivery.Should().Be(restaurant.HasDelivery);
        result.City.Should().Be(restaurant.Address.City);
        result.Street.Should().Be(restaurant.Address.Street);
        result.PostalCode.Should().Be(restaurant.Address.PostalCode);

    }

    [Fact]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // Arrange

        var createRestaurantCommand = new CreateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Test Category",
            HasDelivery = true,
            City = "Test City",
            Street = "Test Street",
            PostalCode = "Test Postal Code"
        };
        // Act
        var result = _mapper.Map<Restaurant>(createRestaurantCommand);
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(createRestaurantCommand.Name);
        result.Description.Should().Be(createRestaurantCommand.Description);
        result.Category.Should().Be(createRestaurantCommand.Category);
        result.HasDelivery.Should().Be(createRestaurantCommand.HasDelivery);
        result.Address.City.Should().Be(createRestaurantCommand.City);
        result.Address.Street.Should().Be(createRestaurantCommand.Street);
        result.Address.PostalCode.Should().Be(createRestaurantCommand.PostalCode);
    }

    [Fact]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // Arrange
        var updateRestaurantCommand = new UpdateRestaurantCommand
        {
            Id = 1,
            Name = "Test Restaurant",
            Description = "Test Description",
            HasDelivery = true,
        };
        // Act
        var result = _mapper.Map<Restaurant>(updateRestaurantCommand);
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(updateRestaurantCommand.Id);
        result.Name.Should().Be(updateRestaurantCommand.Name);
        result.Description.Should().Be(updateRestaurantCommand.Description);
        result.HasDelivery.Should().Be(updateRestaurantCommand.HasDelivery);
    }
}