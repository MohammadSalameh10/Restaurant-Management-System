using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IDashboardService
    {
        AdminDashboardResponse GetSummary();
    }
}
