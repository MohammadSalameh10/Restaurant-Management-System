using Mapster;
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

        public async Task<List<CustomerResponse>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Adapt<List<CustomerResponse>>();
        }

        public async Task<CustomerResponse?> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return null;

            return customer.Adapt<CustomerResponse>();
        }

        public async Task<bool> CreateAsync(CustomerRequest request)
        {
            if (request == null)
                return false;

            var entity = new Customer
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                LocationId = request.LocationId
            };

            await _customerRepository.AddAsync(entity);
            await _customerRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(int id, CustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return false;

            customer.Name = request.Name;
            customer.PhoneNumber = request.PhoneNumber;
            customer.LocationId = request.LocationId;

            await _customerRepository.UpdateAsync(customer);
            await _customerRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return false;

            await _customerRepository.DeleteAsync(customer);
            await _customerRepository.SaveAsync();

            return true;
        }
    }
}
