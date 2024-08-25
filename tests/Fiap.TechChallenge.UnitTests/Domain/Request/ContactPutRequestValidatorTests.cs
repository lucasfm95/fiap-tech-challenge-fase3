using AutoFixture;
using Fiap.TechChallenge.Domain.Request;
using FluentAssertions;

namespace Fiap.TechChallenge.UnitTests.Domain.Request;

public class ContactPutRequestValidatorTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void ShouldReturnErrorWhenEmailIsInvalid()
    {
        // Arrange
        var request = _fixture.Build<ContactPutRequest>()
            .With(x => x.Ddd, 11)
            .With(x => x.PhoneNumber, "123456789")
            .With(x => x.Email, "teste_arroba_gmail.com")
            .Create();

        var requestValidator = new ContactPutRequestValidator();

        //Act
        var requestValidatorResult = requestValidator.Validate(request);

        //Assert
        requestValidatorResult.IsValid.Should().BeFalse();
        requestValidatorResult.Errors.Should().HaveCount(1);
        requestValidatorResult.Errors.Should().Contain(x => x.ErrorMessage == "Invalid email format");
    }
    
    [Fact]
    public void ShouldReturnErrorWhenDddIsNull()
    {
        // Arrange
        var request = _fixture.Build<ContactPutRequest>()
            .With(x => x.Ddd, 0)
            .With(x => x.PhoneNumber, "123456789")
            .With(x => x.Email, "teste_arroba@gmail.com")
            .Create();

        var requestValidator = new ContactPutRequestValidator();

        //Act
        var requestValidatorResult = requestValidator.Validate(request);

        //Assert
        requestValidatorResult.IsValid.Should().BeFalse();
        requestValidatorResult.Errors.Should().HaveCount(1);
        requestValidatorResult.Errors.Should().Contain(x => x.ErrorMessage == "DDD is required");
    }
    
    [Theory]
    [InlineData(100)]
    [InlineData(101)]
    [InlineData(1010)]
    public void ShouldReturnErrorWhenDddIsInvalid(int ddd)
    {
        // Arrange
        var request = _fixture.Build<ContactPutRequest>()
            .With(x => x.Ddd, ddd)
            .With(x => x.PhoneNumber, "123456789")
            .With(x => x.Email, "teste_arroba@gmail.com")
            .Create();

        var requestValidator = new ContactPutRequestValidator();

        //Act
        var requestValidatorResult = requestValidator.Validate(request);

        //Assert
        requestValidatorResult.IsValid.Should().BeFalse();
        requestValidatorResult.Errors.Should().HaveCount(1);
        requestValidatorResult.Errors.Should().Contain(x => x.ErrorMessage == "invalid ddd number");
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("12345678")]
    [InlineData("1234567890")]
    public void ShouldReturnErrorWhenPhoneNumberIsInvalid(string phoneNumber)
    {
        // Arrange
        var request = _fixture.Build<ContactPutRequest>()
            .With(x => x.Ddd, 11)
            .With(x => x.PhoneNumber, phoneNumber)
            .With(x => x.Email, "teste_arroba@gmail.com")
            .Create();

        var requestValidator = new ContactPutRequestValidator();

        //Act
        var requestValidatorResult = requestValidator.Validate(request);

        //Assert
        requestValidatorResult.IsValid.Should().BeFalse();
        requestValidatorResult.Errors.Should().HaveCount(1);
        requestValidatorResult.Errors.Should().Contain(x => x.ErrorMessage == "Phone number must contain 9 digits");
    }
    
    [Fact]
    public void ShouldReturnErrorWhenLengthNameIsLessThan3()
    {
        // Arrange
        var request = _fixture.Build<ContactPutRequest>()
            .With(x => x.Ddd, 11)
            .With(x => x.PhoneNumber, "123456789")
            .With(x => x.Email, "teste_arroba@gmail.com")
            .With(x => x.Name, "12")
            .Create();

        var requestValidator = new ContactPutRequestValidator();

        //Act
        var requestValidatorResult = requestValidator.Validate(request);

        //Assert
        requestValidatorResult.IsValid.Should().BeFalse("The length of 'Name' must be 200 characters or fewer.");
        requestValidatorResult.Errors.Should().HaveCount(1);
        requestValidatorResult.Errors.Should().Contain(x => x.ErrorMessage == "The length of 'Name' must be at least 3 characters.");
    }
    
    [Fact]
    public void ShouldReturnErrorWhenLengthNameIsMoreThan200()
    {
        // Arrange
        var request = _fixture.Build<ContactPutRequest>()
            .With(x => x.Ddd, 11)
            .With(x => x.PhoneNumber, "123456789")
            .With(x => x.Email, "teste_arroba@gmail.com")
            .With(x => x.Name, "123141241242123114124123412412421231412412421231141241234124124212314124124212311412412341241242123141241242123114124123412412421231412412421231141241234124124212314124124212311412412341241242123141241242123114124123412412421231412412421231141241234124124212314124124212311412412341241242123141241242123114124123412412421231412412421231141241234124124212314124124212311412412341241242123141241242123114124123412412421231412412421231141241234124124212314124124212311412412341241242")
            .Create();

        var requestValidator = new ContactPutRequestValidator();

        //Act
        var requestValidatorResult = requestValidator.Validate(request);

        //Assert
        requestValidatorResult.IsValid.Should().BeFalse();
        requestValidatorResult.Errors.Should().HaveCount(1);
        requestValidatorResult.Errors.Should().Contain(x => x.ErrorMessage == "The length of 'Name' must be 200 characters or fewer.");
    }
}