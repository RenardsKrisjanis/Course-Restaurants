﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IUserContext userContext) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        logger.LogInformation("User {UserId} is creating a new restaurant {@Restaurant}", currentUser!.UserId, request);



        logger.LogInformation("Creating a new restaurant {@Restaurant}", request);

        var restaurant = mapper.Map<Restaurant>(request);

        restaurant.OwnerId = currentUser.UserId;

        int id = await restaurantsRepository.Create(restaurant);
        return id;
    }
}
