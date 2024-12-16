using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        _handler = new UpdateRestaurantCommandHandler(_loggerMock.Object,
            _restaurantsRepositoryMock.Object,
            _mapperMock.Object,
            _restaurantAuthorizationServiceMock.Object);
    }

    [Fact()]
    public async Task Handle_WithValidRequest_UpdatesRestaurant()
    {
        // Arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand
        {
            Id = restaurantId,
            Name = "Updated Name",
            Description = "Updated Description",
            HasDelivery = true
        };

        var restaurant = new Restaurant
        {
            Id = restaurantId,
            Name = "test",
            Description = "test",
        };

        _restaurantsRepositoryMock.Setup(x => x.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant); 

        _restaurantAuthorizationServiceMock.Setup(x => x.Authorize(restaurant, ResourceOperation.Update)).Returns(true);

        // Act

        await _handler.Handle(command, CancellationToken.None);

        // Assert

        _mapperMock.Verify(x => x.Map(command, restaurant), Times.Once);
        _restaurantsRepositoryMock.Verify(x => x.SaveChanges(), Times.Once);

    }

    [Fact()]
    public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
    {
        // Arrange
        var restaurantId = 2;
        var command = new UpdateRestaurantCommand
        {
            Id = restaurantId,
        };

        _restaurantsRepositoryMock.Setup(x => x.GetByIdAsync(restaurantId)).ReturnsAsync((Restaurant?)null);
        // Act

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }  

    [Fact()]
    public async Task Handle_WithUnauthorizedUser_ShouldThrowForbiddenAccessException()
    {
        // Arrange
        var restaurantId = 3;
        var request = new UpdateRestaurantCommand
        {
            Id = restaurantId,
        };
        var existingRestaurant = new Restaurant
        {
            Id = restaurantId,
            Name = "test",
            Description = "test",
        };

        _restaurantsRepositoryMock.Setup(x => x.GetByIdAsync(restaurantId)).ReturnsAsync(existingRestaurant);
        _restaurantAuthorizationServiceMock.Setup(x => x.Authorize(existingRestaurant, ResourceOperation.Update)).Returns(false);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

}