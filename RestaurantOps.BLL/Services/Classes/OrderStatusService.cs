using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly IOrderStatusRepository _orderStatusRepository;

        public OrderStatusService(IOrderStatusRepository orderStatusRepository)
        {
            _orderStatusRepository = orderStatusRepository;
        }

        public List<OrderStatusResponse> GetAll()
        {
            var list = _orderStatusRepository.GetAll();
            return list.Select(s => new OrderStatusResponse
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }

        public OrderStatusResponse GetById(int id)
        {
            var s = _orderStatusRepository.GetById(id);
            if (s == null) return null;

            return new OrderStatusResponse
            {
                Id = s.Id,
                Name = s.Name
            };
        }

        public bool Create(OrderStatusRequest request)
        {
            if (request == null) return false;

            var entity = new OrderStatus
            {
                Name = request.Name
            };

            _orderStatusRepository.Add(entity);
            _orderStatusRepository.Save();
            return true;
        }

        public bool Update(int id, OrderStatusRequest request)
        {
            var entity = _orderStatusRepository.GetById(id);
            if (entity == null) return false;

            entity.Name = request.Name;

            _orderStatusRepository.Update(entity);
            _orderStatusRepository.Save();
            return true;
        }

        public bool Delete(int id)
        {
            var entity = _orderStatusRepository.GetById(id);
            if (entity == null) return false;

            _orderStatusRepository.Delete(entity);
            _orderStatusRepository.Save();
            return true;
        }
    }
}
