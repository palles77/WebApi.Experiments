using LpApi_20210506.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace LpApi_20210506.DataAccess
{
    public class DatabaseRepository :  IDatabaseRepository
    {
        private readonly MemoryDatabase _memoryDatabase;

        public DatabaseRepository(MemoryDatabase memoryDatabase)
        {
            _memoryDatabase = memoryDatabase;
        }
        
        public bool SaveCustomer(ICustomer customer)
        {
            bool result = false;
            var concreteCustomer = customer as Customer;
            if (!ReferenceEquals(concreteCustomer, null))
            {
                _memoryDatabase.Customers.Add(concreteCustomer);
                result = _memoryDatabase.SaveChanges() == 1;
            }

            return result;
        }

        public ICustomer GetCustomer(string mainKey)
        {
            var result = _memoryDatabase.Customers
                .FirstOrDefault(x => Equals(x.MainKey, mainKey));

            return result;
        }

        public List<ICustomer> GetCustomers()
        {
            var result = _memoryDatabase.Customers.ToList<ICustomer>();
            return result;
        }
    }
}
