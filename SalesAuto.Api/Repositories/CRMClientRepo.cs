using DataAccessLibrary;
using DB;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SalesAuto.Models.Entities.CRM;
using SalesAuto.Models.Entities.HenKham;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{    


    public class CRMClientRepo : ICRMClientRepo
    {
        private readonly IConfiguration config;
        private readonly SalesAutoDbContext context;
        private readonly ISqlDataAccess sqlDataAccess;
        HttpClient client;
        public static LoginSuccess LoginInfo;
        public CRMClientRepo(IConfiguration config, IHttpClientFactory client_httpClientFactory, SalesAutoDbContext context, ISqlDataAccess sqlDataAccess)
        {
            this.config = config;
            this.context = context;
            this.sqlDataAccess = sqlDataAccess;
            client = client_httpClientFactory.CreateClient("CRMClientRep");            
            if (LoginInfo!=null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginInfo.access_token);
            }            
        }

        public async Task<bool> Loggin(bool force=false)
        {
            if (force || LoginInfo == null || string.IsNullOrEmpty(LoginInfo.access_token))
            {
                try
                {

                    var byteArray = Encoding.ASCII.GetBytes(config.GetSection("CRMAPI")["Username"] + ":" + config.GetSection("CRMAPI")["Password"]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    var result = await client.GetAsync("/apiv1/auth");
                    var content = await result.Content.ReadAsStringAsync();
                    if (content.IndexOf("access_token") >= 0)
                    {
                        LoginInfo = System.Text.Json.JsonSerializer.Deserialize<LoginSuccess>(content,
                           new JsonSerializerOptions()
                           {
                               PropertyNameCaseInsensitive = true
                           });
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", LoginInfo.access_token);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginInfo.access_token);
                      
                        return true; ;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                if (LoginInfo != null && !string.IsNullOrEmpty(LoginInfo.access_token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginInfo.access_token);
                }
            }
            return true;
        }
        public async Task<string> order_create(OrderCreate order)
        {
            if (await Loggin())
            {
                order.email = "infor@matsaigon.com";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsJsonAsync($"/tcrm/order/create", order);
                if (response.StatusCode.ToString() == "401" || response.ReasonPhrase.Contains("Unau"))
                {
                    if (response.ReasonPhrase != "" && response.ReasonPhrase.Contains("Unau"))
                    {
                        await Loggin(true);
                        response = await client.PostAsJsonAsync($"/tcrm/order/create", order);
                    }
                }
                if (response == null)
                {
                    return "";
                }
                if (response.IsSuccessStatusCode)
                {
                    var contenta = await response.Content.ReadAsStringAsync();
                    var result = System.Text.Json.JsonSerializer.Deserialize<ResponseResult>(contenta,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                       
                    if (result.result)
                    {
                        return result.message;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
               
            }
            return "";
        }

        public async Task<bool> order_update(OrderUpdate orderUpdate)
        {
            if (await Loggin())
            {                
                var response = await client.PostAsJsonAsync($"/tcrm/order/update", orderUpdate);
                if (response.StatusCode.ToString() == "401" || response.ReasonPhrase.Contains("Unau"))
                {
                    if (response.ReasonPhrase != "" && response.ReasonPhrase.Contains("Unau"))
                    {
                        await Loggin(true);
                        response = await client.PostAsJsonAsync($"/tcrm/order/update", orderUpdate);
                    }
                }
                if (response == null)
                {
                    return false;
                }
                if (response.IsSuccessStatusCode)
                {
                    var contenta = await response.Content.ReadAsStringAsync();
                    var result = System.Text.Json.JsonSerializer.Deserialize<ResponseResult>(contenta,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );                    
                    if (result.result)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }


        public async Task<OrderResultAPI> Getorder(DateTime TuNgay, DateTime DenNgay, int Trang = 0)
        {
            if (await Loggin())
            {
                var queryStringParam = new Dictionary<string, string>
                {
                    ["order_date_from"] = TuNgay.ToString("yyyy-MM-dd"),
                    ["order_date_to"] = DenNgay.ToString("yyyy-MM-dd"),
                    ["page"] ="1"
                };
                string url = QueryHelpers.AddQueryString("/tcrm/order", queryStringParam);
                var result = await client.GetAsync(url);

                if (result.StatusCode.ToString() == "401" || result.ReasonPhrase.Contains("Unau"))
                {
                    if (result.ReasonPhrase != "" && result.ReasonPhrase.Contains("Unau"))
                    {
                        await Loggin(true);
                        result = await client.GetAsync(url);
                    }
                }
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var orderRe = System.Text.Json.JsonSerializer.Deserialize<OrderResultAPI>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    await ChuyenLichKhamVeBenhVien(orderRe.data);

                    if (orderRe.meta.totalPage>1)
                    {
                        for (int SoTrang = 2; SoTrang <= orderRe.meta.totalPage; SoTrang++) 
                        {
                            try
                            {
                                var queryStringTheoTrang = new Dictionary<string, string>
                                {
                                    ["order_date_from"] = TuNgay.ToString("yyyy-MM-dd"),
                                    ["order_date_to"] = DenNgay.ToString("yyyy-MM-dd"),
                                    ["page"] = ("" + SoTrang)
                                };
                                string urlTheoTrang = QueryHelpers.AddQueryString("/tcrm/order", queryStringTheoTrang);
                                var resultTrang = await client.GetAsync(urlTheoTrang);
                                if (resultTrang.IsSuccessStatusCode)
                                {
                                    var contentTrang = await resultTrang.Content.ReadAsStringAsync();
                                    var orderReTrang = System.Text.Json.JsonSerializer.Deserialize<OrderResultAPI>(contentTrang,
                                        new JsonSerializerOptions()
                                        {
                                            PropertyNameCaseInsensitive = true
                                        });
                                    await ChuyenLichKhamVeBenhVien(orderReTrang.data);
                                }
                            }
                            catch (Exception ex)
                            {
                                
                            }

                        }
                    }
                    return orderRe;
                }
                return null;
            }
            return null;
        }

        public async Task ChuyenLichKhamVeBenhVien(List<OrderResult> data)
        {
            var tbl = await sqlDataAccess.getDataTable("select CRM_store_code, preSQL from BenhVien Where LEN(isnull(CRM_store_code, '')) > 0 and LEN(isnull(preSQL, '')) > 0 ");
            foreach (DataRow row in tbl.Rows)
            {
                string preSQL = row["preSQL"].ToString();
                string CRM_store_code = row["CRM_store_code"].ToString();
                var found = data.FindAll(x => x.store_code == CRM_store_code);

                foreach (var item in found)
                {
                    var DiaChi = "";// item.address;
                    ProductOrderResult card = (item.cart_list != null && item.cart_list.Count > 0 ? item.cart_list[0] : null);
                    string pro_code = "";
                    if (card != null)
                    {
                        if (card.unit_code == null)
                        {
                            card.unit_code = "";
                        }

                        if (string.IsNullOrEmpty(card.unit_code))
                        {
                            var procduct = await context.CRM_Products.FirstOrDefaultAsync(x => x.name == card.unit_name);
                            if (procduct != null)
                            {
                                pro_code = procduct.product_code;
                            }
                        }
                        else
                        {
                            pro_code = card.unit_code;
                        }
                    }
                    if (String.IsNullOrEmpty(DiaChi))
                    {
                        var xa =  await context.CRM_Wards.FirstOrDefaultAsync(x => x.id_ward == item.id_ward);
                        if (xa != null)
                        {
                            DiaChi = DiaChi + xa.name;
                        }
                        var huyen = await context.CRM_Districts.FirstOrDefaultAsync(x => x.id_district == item.id_district);
                        if (huyen != null)
                        {
                            DiaChi = DiaChi + (string.IsNullOrEmpty(DiaChi) ? "":", ") + huyen.name;
                        }
                        var Tinh = await context.CRM_Provinces.FirstOrDefaultAsync(x => x.id_province == item.id_province);
                        if (Tinh != null)
                        {
                            if (DiaChi==null)
                            {
                                DiaChi = "";
                            }
                            DiaChi = DiaChi + (string.IsNullOrEmpty(DiaChi) ? "" : ", ") + Tinh.name;
                        }
                    }
                    var sql = "exec " + preSQL + @"proc_CRMSaveDSHenKham 
                        null
                        ,N'" + item.customer_name + @"'
                        , null
	                    , null
                        , N'" + item.customer_phone + @"'
                        , N'" + DiaChi + @"'
                        , '" + item.order_date + @"'
                        , '" + item.order_date + @"'
                        , N'" + (await LayLoaiKham(item)) + @"'
                        , null
                        , null
                        ," + item.id_order + @"
                        , null
                        , null"
                        + "," + (pro_code == "" ? "null" : "'" + pro_code + "'") + ""
                        + "," + (item.order_status == null ? "null" : item.order_status.ToString());//@id_order_status;
                    await sqlDataAccess.execNonQuery(sql);
                }
            }
        }

        public async Task<string> LayLoaiKham(OrderResult item)
        {
            if (item.cart_list != null && item.cart_list.Count >= 1)
            {
                var a = item.cart_list[0];
                var found = (await context.CRM_Products.ToListAsync()).Find(x => x.product_code == a.unit_code);
                if (found != null)
                {
                    context.Entry(found).Reference(s => s.category_info).Load(); // loads StudentAddress
                    if (found.category_info != null)
                    {
                        return found.category_info.category_name + ";" + a.unit_name;
                    }
                    else
                    {
                        return a.unit_name;
                    }
                }
                else
                {
                    return a.unit_name;
                }
            }
            else
            {
                return "Chưa xác định";
            }
        }

        public async Task<Location> Getlocation()
        {
            if (await Loggin())
            {
                var result = await client.GetAsync("/tcrm/location");
                if (result.StatusCode.ToString() == "401" || result.ReasonPhrase.Contains("Unau"))
                {
                    if (result.ReasonPhrase != "" && result.ReasonPhrase.Contains("Unau"))
                    {
                        await Loggin(true);
                        result = await client.GetAsync("/tcrm/location");
                    }
                }
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var kq = System.Text.Json.JsonSerializer.Deserialize<Location>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    if (kq != null)
                    {
                        if (kq.provinces != null)
                        {
                            foreach (var item in kq.provinces)
                            {
                                var catefound = await context.CRM_Provinces.FirstOrDefaultAsync(x => x.id_province == item.id_province);
                                if (catefound == null)
                                {
                                    context.CRM_Provinces.Add(new Province
                                    {
                                        id_province = item.id_province,
                                        name = item.name
                                    });
                                    await context.SaveChangesAsync();
                                }
                                else
                                {
                                    catefound.name = item.name;
                                    await context.SaveChangesAsync();
                                }
                            }
                            foreach (var item in kq.districts)
                            {
                                var catefound = await context.CRM_Districts.FirstOrDefaultAsync(x => x.id_district == item.id_district);
                                if (catefound == null)
                                {
                                    context.CRM_Districts.Add(new District
                                    {
                                        id_district = item.id_district,
                                        name = item.name
                                    });
                                    await context.SaveChangesAsync();
                                }
                                else
                                {
                                    catefound.name = item.name;
                                    await context.SaveChangesAsync();
                                }
                            }
                            foreach (var item in kq.wards)
                            {
                                var catefound = await context.CRM_Wards.FirstOrDefaultAsync(x => x.id_ward == item.id_ward);
                                if (catefound == null)
                                {
                                    context.CRM_Wards.Add(new Ward
                                    {
                                        id_ward = item.id_ward,
                                        name = item.name
                                    });
                                    await context.SaveChangesAsync();
                                }
                                else
                                {
                                    catefound.name = item.name;
                                    await context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
                return null;
            }
            return null;
        }

        public async Task<List<Product>> Getproduct()
        {
            if (await Loggin())
            {
                var result = await client.GetAsync("/tcrm/product");
                if (result.StatusCode.ToString() == "401" || result.ReasonPhrase.Contains("Unau"))
                {
                    if (result.ReasonPhrase != "" && result.ReasonPhrase.Contains("Unau"))
                    {
                        await Loggin(true);
                        result = await client.GetAsync("/tcrm/product");
                    }
                }
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var procducts = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    foreach (var item in procducts)
                    {
                        var catefound = await context.CRM_Categories.FirstOrDefaultAsync(x => x.id_category == item.category_info.id_category);
                        if (catefound == null)
                        {
                            context.CRM_Categories.Add(new Category { id_category=item.category_info.id_category,
                                category_name = item.category_info.category_name
                            });
                            await context.SaveChangesAsync();
                        }
                        //else
                        //{
                        //    catefound.category_name = item.category_info.category_name;
                        //}
                        var typefound = await context.CRM_Types.FirstOrDefaultAsync(x => x.id_type == item.type_info.id_type);
                        if(typefound == null)
                        {
                            context.CRM_Types.Add(new Models.Entities.CRM.Type { id_type = item.type_info.id_type,
                                type_name = item.type_info.type_name
                            });
                            await context.SaveChangesAsync();
                        }                                           
                    }
                    
                   

                    foreach (var item in procducts)
                    {                        
                        var found = await context.CRM_Products.FirstOrDefaultAsync(x => x.id_product == item.id_product);      
                        
                        if (found == null)
                        {  
                            var catefound = await context.CRM_Categories.FirstOrDefaultAsync(x => x.id_category == item.category_info.id_category);
                            if (catefound != null)
                            {
                                item.category_info = catefound;
                            }
                            var typefound = await context.CRM_Types.FirstOrDefaultAsync(x => x.id_type == item.type_info.id_type);
                            if (typefound != null)
                            {
                                item.type_info = typefound;
                            }

                            context.CRM_Products.Add(item);
                            context.Entry(item.category_info).State = EntityState.Detached;
                            context.Entry(item.type_info).State = EntityState.Detached;

                        }
                        else
                        {
                            found.name = item.name;
                            found.product_code = item.product_code;
                            found.product_price = item.product_price;

                            context.Entry(item.category_info).State = EntityState.Detached;
                            context.Entry(item.type_info).State = EntityState.Detached;
                            
                        }                        
                        await context.SaveChangesAsync();
                    }                  
                    return procducts;                         
                }
                return null;
            }
            return null;
        }

        public async Task<List<Store>> GetStore()
        {
            if (await Loggin())
            {
                var result = await client.GetAsync("/tcrm/store");
                if (result.StatusCode.ToString() == "401" || result.ReasonPhrase.Contains("Unau"))
                {
                    if (result.ReasonPhrase != "" && result.ReasonPhrase.Contains("Unau"))
                    {
                        await Loggin(true);
                        result = await client.GetAsync("/tcrm/store");
                    }
                }
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var stores = System.Text.Json.JsonSerializer.Deserialize<List<Store>>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    foreach (var store in stores)
                    {
                        var found = await context.CRM_Stores.FirstOrDefaultAsync(x => x.id_store == store.id_store);
                        if (found == null)
                        {
                            context.CRM_Stores.Add(store);
                        }
                        else
                        {
                            found.name = store.name;
                            found.address = store.address;
                            found.phone = store.phone;
                        }
                        await context.SaveChangesAsync();
                    }
                    
                    return stores;
                }
                return null;
            }
            return null;
        }

        public async Task<List<Order_status>> Getorder_status()
        {
            if (await Loggin())
            {
                var result = await client.GetAsync("/tcrm/order-status");
                if (result.StatusCode.ToString() == "401" || result.ReasonPhrase.Contains("Unau"))
                {
                    if (result.ReasonPhrase != "" && result.ReasonPhrase.Contains("Unau"))
                    {
                        await Loggin(true);
                        result = await client.GetAsync("/tcrm/order-status");
                    }
                }
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var Order_statuss = System.Text.Json.JsonSerializer.Deserialize<List<Order_status>>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    foreach (var item in Order_statuss)
                    {
                        var found = await context.CRM_Order_status.FirstOrDefaultAsync(x => x.order_status == item.order_status);
                        if (found == null)
                        {
                            context.CRM_Order_status.Add(item);
                        }
                        else
                        {
                            found.name = item.name;
                            found.description = item.description;
                        }
                        await context.SaveChangesAsync();
                    }

                    return Order_statuss;
                }
                return null;
            }
            return null;
        }
        
        public async Task<OrderResult> GetorderByCRM_id_order(int CRM_id_order)
        {
            if (await Loggin())
            {
                var queryStringParam = new Dictionary<string, string>
                {
                    ["id_order"] = ""+CRM_id_order
                };
                string url = QueryHelpers.AddQueryString("/tcrm/order", queryStringParam);

                var result = await client.GetAsync(url);
                if (result.StatusCode.ToString() == "401" || result.ReasonPhrase.Contains("Unau"))
                {
                    if(result.ReasonPhrase!="" && result.ReasonPhrase.Contains("Unau"))
                    {
                        await Loggin(true);
                        result = await client.GetAsync(url);
                    }
                }
                if (result.IsSuccessStatusCode)
                {
                    try
                    {
                        var content = await result.Content.ReadAsStringAsync();
                        var orderRe = System.Text.Json.JsonSerializer.Deserialize<OrderResultAPI>(content,
                            new JsonSerializerOptions()
                            {
                                PropertyNameCaseInsensitive = true,
                                IgnoreNullValues = true,
                                IgnoreReadOnlyFields = true,

                            });
                        if (orderRe.data != null && orderRe.data.Count > 0)
                        {
                            return orderRe.data.FirstOrDefault();
                        }
                    }
                    catch
                    {
                        return new OrderResult() { id_order = CRM_id_order };
                    }
                    return null;                    
                }
                return null;
            }
            return null;
        }

    }
}
