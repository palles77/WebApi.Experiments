using LpApi_20210506.Common;

namespace LpApi_20210506.Interfaces
{
    public interface ICustomer
    {
        string Street { get; set; }
        string HouseNumber { get; set; }
        string ZipCode { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        CustomerSourceEnum CustomerSource { get; set; }
        string PersonalNumber { get; set; }
        string FavoriteFootballTeam { get; set; }
    }
}
