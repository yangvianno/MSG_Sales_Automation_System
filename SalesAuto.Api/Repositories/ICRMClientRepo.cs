using SalesAuto.Models.Entities.CRM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface ICRMClientRepo
    {
        Task<Location> Getlocation();
        Task<OrderResultAPI> Getorder(DateTime TuNgay, DateTime DenNgay, int Trang = 0);
        Task<OrderResult> GetorderByCRM_id_order(int CRM_id_order);
        Task<List<Order_status>> Getorder_status();
        Task<List<Product>> Getproduct();
        Task<List<Store>> GetStore();
        Task<bool> Loggin(bool force = false);
        Task<string> order_create(OrderCreate order);
        Task<bool> order_update(OrderUpdate orderUpdate);
    }
}