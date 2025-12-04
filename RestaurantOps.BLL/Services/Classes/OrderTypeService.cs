using Mapster;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class OrderTypeService : IOrderTypeService
    {
        private readonly IOrderTypeRepository _orderTypeRepository;

        public OrderTypeService(IOrderTypeRepository orderTypeRepository)
        {
            _orderTypeRepository = orderTypeRepository;
        }

        public async Task<List<OrderTypeResponse>> GetAllAsync()
        {
            var list = await _orderTypeRepository.GetAllAsync();
            return list.Adapt<List<OrderTypeResponse>>();
        }

        public async Task<OrderTypeResponse?> GetByIdAsync(int id)
        {
            var type = await _orderTypeRepository.GetByIdAsync(id);
            return type?.Adapt<OrderTypeResponse>();
        }

        public async Task<bool> CreateAsync(OrderTypeRequest request)
        {
            if (request == null)
                return false;

            var entity = request.Adapt<OrderType>();

            await _orderTypeRepository.AddAsync(entity);
            await _orderTypeRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(int id, OrderTypeRequest request)
        {
            var entity = await _orderTypeRepository.GetByIdAsync(id);
            if (entity == null)
                return false;

            request.Adapt(entity);

            await _orderTypeRepository.UpdateAsync(entity);
            await _orderTypeRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _orderTypeRepository.GetByIdAsync(id);
            if (entity == null)
                return false;

            await _orderTypeRepository.DeleteAsync(entity);
            await _orderTypeRepository.SaveAsync();

            return true;
        }
    }
}
