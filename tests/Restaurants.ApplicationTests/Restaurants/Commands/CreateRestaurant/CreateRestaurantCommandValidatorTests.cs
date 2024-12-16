using FluentValidation.TestHelper;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Test",
            Category = "Italian",
            ContactEmail = "Test@test.com",
            PostalCode = "12-123",
        };

        var validator = new CreateRestaurantCommandValidator();


        // Act
        var result = validator.TestValidate(command);

        // Assert

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInValidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Te",
            Category = "Ita",
            ContactEmail = "Test",
            PostalCode = "12123",
        };

        var validator = new CreateRestaurantCommandValidator();

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Category);
        result.ShouldHaveValidationErrorFor(x => x.ContactEmail);
        result.ShouldHaveValidationErrorFor(x => x.PostalCode);
    }

    [Theory()]
    [InlineData("Italian")]
    [InlineData("Indian")]
    [InlineData("American")]
    [InlineData("Japanese")]
    [InlineData("Mexican")]
    public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoryProperty(string category)
    {
        // Arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand { Category = category };

        // Act

        var result = validator.TestValidate(command);

        // Assert

        result.ShouldNotHaveValidationErrorFor(x => x.Category);

    }

    [Theory()]
    [InlineData("12345")]
    [InlineData("123-23")]
    [InlineData("12 123")]
    [InlineData("1-2 23")]
    public void Validator_ForInvalidPostalCode_ShouldHaveValidationErrorsForPostalCodeProperty(string postalCode)
    {
        // Arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand { PostalCode = postalCode };
        // Act
        var result = validator.TestValidate(command);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PostalCode);
    }
}