﻿using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Users.Tests;

public class UserContextTests
{
    [Fact()]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        // Arrange
        var dateOfBirth = new DateOnly(1990, 1, 1);
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@test.com"),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User),
            new("Nationality", "German"),
            new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
            };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        httpContextAccessorMock.Setup(x => x.HttpContext.User).Returns(user);

        var userContext = new UserContext(httpContextAccessorMock.Object);

        // Act

        var currentUser = userContext.GetCurrentUser();

        // Assert

        currentUser.Should().NotBeNull();
        currentUser!.UserId.Should().Be("1");

        currentUser.Email.Should().Be("test@test.com");
        currentUser.Roles.Should().ContainEquivalentOf(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be("German");
        currentUser.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact()]
    public void GetCurrentUser_WithUserContextBotPresent_ThrowsInvalidOperationException()
    {
        // Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);
        var userContext = new UserContext(httpContextAccessorMock.Object);
        // Act
        Action action = () => userContext.GetCurrentUser();
        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

}