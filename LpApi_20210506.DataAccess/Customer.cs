using LpApi_20210506.Common;
using LpApi_20210506.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LpApi_20210506.DataAccess
{
    public class Customer : ICustomer
    {
        #region Attributes

        [Required]
        public string Street { get; set; }

        [Required]
        public string HouseNumber { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string FirstName { get;  set; }


        [Required]
        public string LastName { get; set; }

        public string MainKey { get; set; }

        public CustomerSourceEnum CustomerSource { get; set; }
        
        [RequiredIf(nameof(CustomerSource), CustomerSourceEnum.MrGreen,
            ErrorMessageResourceName = "PersonalNumberRequired",
            ErrorMessageResourceType = typeof(SharedResource))]
        public string PersonalNumber { get; set; }
        
        [RequiredIf(nameof(CustomerSource), CustomerSourceEnum.RedBet,
            ErrorMessageResourceName = "FavoriteFootbalTeamRequired",
            ErrorMessageResourceType = typeof(SharedResource))]
        public string FavoriteFootballTeam { get; set; }

        #endregion Attributes

        #region Public Methods

        public IList<ValidationResult> ValidateModel()
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(this, null, null);
            Validator.TryValidateObject(this, ctx, validationResults, true);
            return validationResults;
        }

        #endregion Public Methods

        #region Ctor

        public Customer(string firstName, 
            string lastName, 
            string houseNumber, 
            string street, 
            string zipCode, 
            string personalNumber, 
            string favoriteFootballTeam, 
            CustomerSourceEnum customerSource)
        {
            FirstName = firstName;
            LastName = lastName;
            MainKey = firstName + lastName;
            HouseNumber = houseNumber;
            Street = street;
            ZipCode = zipCode;
            PersonalNumber = personalNumber;
            FavoriteFootballTeam = favoriteFootballTeam;
            CustomerSource = customerSource;
        }

        #endregion Ctor
    }
}
