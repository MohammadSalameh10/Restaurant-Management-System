using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<CustomerResponse> GetAll()
        {
            var customers = _customerRepository.GetAll();
            return customers.Select(MapToResponse).ToList();
        }

        public CustomerResponse GetById(int id)
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null)
                return null;

            return MapToResponse(customer);
        }

        public bool Create(CustomerRequest request)
        {
            if (request == null)
                return false;

            var entity = new Customer
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                LocationId = request.LocationId
            };

            _customerRepository.Add(entity);
            _customerRepository.Save();
            return true;
        }

        public bool Update(int id, CustomerRequest request)
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null)
                return false;

            customer.Name = request.Name;
            customer.PhoneNumber = request.PhoneNumber;
            customer.LocationId = request.LocationId;

            _customerRepository.Update(customer);
            _customerRepository.Save();
            return true;
        }

        public bool Delete(int id)
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null)
                return false;

            _customerRepository.Delete(customer);
            _customerRepository.Save();
            return true;
        }

        private CustomerResponse MapToResponse(Customer customer)
        {
            return new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                LocationId = customer.LocationId,
                LocationCity = customer.Location?.City,
                LocationStreet = customer.Location?.Street
            };
        }
    }
}
