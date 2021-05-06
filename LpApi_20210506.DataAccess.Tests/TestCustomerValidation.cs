using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LpApi_20210506.Common;
using NUnit.Framework;

namespace LpApi_20210506.DataAccess.Tests
{
    public class TestCustomerValidation
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("", true)]
        [TestCase(null, true)]
        [TestCase("    ", true)]
        [TestCase("1231", false)]
        public void TestMrGreenRequiredIf(string personalNumber, bool shouldValidationFail)
        {
            // Arrange
            var customer = new Customer("", "", "", "", "", 
                personalNumber, "", CustomerSourceEnum.MrGreen);

            // Act
            var validateModel = ValidateModel(customer);
            var result = validateModel
                .Any(v => v.ErrorMessage != null &&
                          v.MemberNames.Contains("CustomerNumber") && 
                          v.ErrorMessage.Contains(SharedResource.Required));

            // Assert
            Assert.AreEqual(shouldValidationFail, result);
        }

        [TestCase("", true)]
        [TestCase(null, true)]
        [TestCase("    ", true)]
        [TestCase("1231", false)]
        public void TestRedBetRequiredIf(string favoriteFootballTeam, bool shouldValidationFail)
        {
            // Arrange
            var customer = new Customer("", "", "", "", "", "", 
                favoriteFootballTeam, CustomerSourceEnum.RedBet);

            // Act
            var validateModel = ValidateModel(customer);
            var result = validateModel
                .Any(v => v.ErrorMessage != null &&
                          v.MemberNames.Contains("FavoriteFootballTeam") &&
                          v.ErrorMessage.Contains(SharedResource.Required));

            // Assert
            Assert.AreEqual(shouldValidationFail, result);
        }

        [TestCase("",  "", "", "HouseNumber", true)]
        [TestCase("", "", "", "Street", true)]
        [TestCase("", "", "", "ZipCode", true)]
        [TestCase("12", "", "", "HouseNumber", false)]
        [TestCase("", "Street", "", "Street", false)]
        [TestCase("", "", "40-018", "ZipCode", false)]
        public void TestAddressProperties(string houseNumber, string street, string zipCode, string propertyValidated, bool shouldValidationFail)
        {
            // Arrange
            var customer = new Customer("", "",
                houseNumber, street, zipCode,
                "","", CustomerSourceEnum.MrGreen);

            // Act
            var validateModel = ValidateModel(customer);
            var result = validateModel
                .Any(v => v.ErrorMessage != null &&
                          v.MemberNames.Contains(propertyValidated) &&
                          v.ErrorMessage.ToLower().Contains(SharedResource.Required.ToLower()));

            // Assert
            Assert.AreEqual(shouldValidationFail, result);
        }

        [TestCase("", "", "FirstName", true)]
        [TestCase("", "", "LastName", true)]
        [TestCase("Leslaw", "", "FirstName", false)]
        [TestCase("", "Pawlaczyk", "LastName", false)]
        public void TestNameProperties(string firstName, string lastName, string propertyValidated, bool shouldValidationFail)
        {
            // Arrange
            var customer = new Customer(firstName, lastName,
                "", "", "", "", "", CustomerSourceEnum.MrGreen);

            // Act
            var validateModel = ValidateModel(customer);
            var result = validateModel
                .Any(v => v.ErrorMessage != null &&
                          v.MemberNames.Contains(propertyValidated) &&
                          v.ErrorMessage.ToLower().Contains(SharedResource.Required.ToLower()));

            // Assert
            Assert.AreEqual(shouldValidationFail, result);
        }

        private IList<ValidationResult> ValidateModel(Customer person)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(person, null, null);
            Validator.TryValidateObject(person, ctx, validationResults, true);
            return validationResults;
        }
    }
}