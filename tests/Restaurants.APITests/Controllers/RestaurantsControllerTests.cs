using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using Restaurants.APITests;
using Moq;
using Restaurants.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restaurants.Domain.Entities;
using System.Net.Http.Json;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new ();
    private readonly Mock<IRestaurantSeeder> _restaurantSeederMock = new();

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository), _ => _restaurantsRepositoryMock.Object));

                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantSeeder), _ => _restaurantSeederMock.Object));
            });
        });
    }

    [Fact()]
    public async void GetById_ForNonExistingId_Returns404NotFound()
    {
        // Arrange
        var id = 1123;
        _restaurantsRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);
        var client = _factory.CreateClient();
        // Act
        var result = await client.GetAsync($"/api/restaurants/{id}");
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact()]
    public async void GetById_ForExistingId_Returns200Ok()
    {
        // Arrange
        var id = 99;
        var restaurant = new Restaurant { Id = id, Name = "Test", Description = "Test Description" };
        _restaurantsRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);
        var client = _factory.CreateClient();
        // Act
        var result = await client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto = await result.Content.ReadFromJsonAsync<RestaurantDto>();
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be("Test");
        restaurantDto.Description.Should().Be("Test Description");
    }

    [Fact()]
    public async void GetAll_ForValidRequest_Returns200Ok()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act

        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        // Assert

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact()]
    public async void GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act

        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        // Assert

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}