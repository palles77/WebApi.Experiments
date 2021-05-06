using System.Collections.Generic;
using LpApi_20210506.Common;

namespace LpApi_20210506.Interfaces
{
    public interface IDatabaseRepository
    {
        bool SaveCustomer(ICustomer customer);
        ICustomer GetCustomer(string mainKey);
        List<ICustomer> GetCustomers();
    }
}
