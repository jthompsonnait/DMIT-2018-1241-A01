using HogWildSystem.DAL;
using HogWildSystem.ViewModels;

namespace HogWildSystem.BLL
{
    public class CustomerService
    {
        private readonly HogWildContext _hogWildContext;

        //  Constructor for the CustomerService class.
        internal CustomerService(HogWildContext hogWildContext)
        {
            _hogWildContext = hogWildContext;
        }

        public List<CustomerSearchView> GetCustomers(string lastName, string phone)
        {
            // Business Rules
            // These are processing rules that need to be satisfied
            // for valid data

            // Rule: Both last name and phone number cannot be empty
            // Rule: RemoveFromViewFlag must be false
            if (string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentNullException("Please provide either a last name and/or phone number");
            }

            // Need to update parameters so we are not searching on an empty value.
            // Otherwise, an empty string will return all records
            if (string.IsNullOrWhiteSpace(lastName))
            {
                lastName = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                phone = Guid.NewGuid().ToString();
            }

            return _hogWildContext.Customers
                .Where(x => (x.LastName.Contains(lastName.Trim())
                             || x.Phone.Contains(phone.Trim()))
                            && !x.RemoveFromViewFlag)
                .Select(x => new CustomerSearchView
                {
                    CustomerID = x.CustomerID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    City = x.City,
                    Phone = x.Phone,
                    Email = x.Email,
                    StatusID = x.StatusID,
                    TotalSales = x.Invoices.Sum(x => x.SubTotal + x.Tax)
                })
                .OrderBy(x => x.LastName)
                .ToList();
        }
    }
}
