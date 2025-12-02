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

        public List<OrderTypeResponse> GetAll()
        {
            var list = _orderTypeRepository.GetAll();
            return list.Adapt<List<OrderTypeResponse>>();
        }

        public OrderTypeResponse GetById(int id)
        {
            var type = _orderTypeRepository.GetById(id);
            return type?.Adapt<OrderTypeResponse>();
        }

        public bool Create(OrderTypeRequest request)
        {
            if (request == null) return false;

            var entity = request.Adapt<OrderType>();

            _orderTypeRepository.Add(entity);
            _orderTypeRepository.Save();
            return true;
        }

        public bool Update(int id, OrderTypeRequest request)
        {
            var entity = _orderTypeRepository.GetById(id);
            if (entity == null) return false;

            request.Adapt(entity);

            _orderTypeRepository.Update(entity);
            _orderTypeRepository.Save();
            return true;
        }

        public bool Delete(int id)
        {
            var entity = _orderTypeRepository.GetById(id);
            if (entity == null) return false;

            _orderTypeRepository.Delete(entity);
            _orderTypeRepository.Save();
            return true;
        }
    }
}
