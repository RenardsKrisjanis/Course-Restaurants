using FluentAssertions;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Users.Tests;

public class CurrentUserTests
{
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsinRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        // Act
        var result = currentUser.IsinRole(roleName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact()]
    public void IsinRole_WithNoMatchingRole_ShouldReturnFalse()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        // Act
        var result = currentUser.IsinRole(UserRoles.Owner);

        // Assert
        result.Should().BeFalse();
    }

    [Fact()]
    public void IsinRole_WithNoMatchingRoleCase_ShouldReturnFalse()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        // Act
        var result = currentUser.IsinRole(UserRoles.Owner.ToLower());

        // Assert
        result.Should().BeFalse();
    }
}