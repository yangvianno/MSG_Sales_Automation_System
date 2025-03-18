using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using SalesAuto.Models;
using SalesAuto.Models.Entities;
using SalesAuto.Models.SearchModel;
using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;


namespace SalesAuto.Wasm.Services
{
    public class ABRClient : IABRClient
    {
        private readonly HttpClient httpClient;
        private readonly IAuthService authService;

        public ABRClient(HttpClient httpClient, IAuthService authService )
        {
            this.httpClient = httpClient;
            this.authService = authService;            
        }
        public async Task<List<ABRDanhMuc>> GetDanhMucABR()
        {
            var result = await httpClient.GetFromJsonAsync<List<ABRDanhMuc>>($"/api/ABR/getDanhMucABR");
            return result;
        }
        public async Task<List<ABRDanhMucXetDuyet>> GetDanhMucXetDuyet()
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<List<ABRDanhMucXetDuyet>>($"/api/ABR/GetDanhMucXetDuyet");
                return result;
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<List<ABRDanhMucXetDuyet>> GetDanhMucXetDuyetMaster()
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<List<ABRDanhMucXetDuyet>>($"/api/ABR/GetDanhMucXetDuyetMaster");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<ABRDanhMucXetDuyet> SaveTinhTrangXetDuyetMaster(ABRDanhMucXetDuyet aBRDanhMucXetDuyet)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"/api/ABR/SaveTinhTrangXetDuyetMaster", aBRDanhMucXetDuyet);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ABRDanhMucXetDuyet>();
                    return result;
                }
                else
                {
                    var str = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(str);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<List<ABRNhom>> GetNhomABR()
        {
            var result = await httpClient.GetFromJsonAsync<List<ABRNhom>>($"/api/ABR/getNhomABR");
            return result;
        }
        public async Task<List<string>> GetNhomCongViecThongKe()
        {
            var result = await httpClient.GetFromJsonAsync<List<string>>($"/api/ABR/GetNhomCongViecThongKe");
            return result;
        }
        

        public async Task<List<ABRCongViecHisVM>> GetDanhMucCongViecHis()
        {
            var result = await httpClient.GetFromJsonAsync<List<ABRCongViecHisVM>>($"/api/ABR/getDanhMucCongViecHis");
            return result;
        }

        public async Task<bool> SaveDanhMucABR(ABRDanhMuc aBRDanhMuc)
        {

            var response = await httpClient.PostAsJsonAsync($"/api/ABR/saveDanhMucABR", aBRDanhMuc);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteDanhMucABR(int iD)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/deleteDanhMucABR?iD=" + iD, iD);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }
        public async Task<ABRMapCongViecABRHIS> SaveMapCongViecABRHIS(ABRMapCongViecABRHIS item)
        { 
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/saveMapCongViecABRHIS", item);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ABRMapCongViecABRHIS>();
                return result;
            }
            else
            {
                return item;
            }
        }
        public async Task<bool> DeleteMapCongViecABRHIS(int iD)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/deleteMapCongViecABRHIS?iD=" + iD, iD);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
            
        }
        public async Task<List<ABRMapCongViecABRHISVM>> GetDanhSachMapCongViecABRHIS()
        {
            var result = await httpClient.GetFromJsonAsync<List<ABRMapCongViecABRHISVM>>($"/api/ABR/getDanhSachMapCongViecABRHIS");
            return result;
        }

        #region MaterData
        public async Task<List<ABRDanhMuc>> GetDanhMucABRMasterData()
        {
            var result = await httpClient.GetFromJsonAsync<List<ABRDanhMuc>>($"/api/ABR/GetDanhMucABRMasterData");
            return result;
        }

        public async Task<bool> SaveDanhMucABRMasterData(ABRDanhMuc aBRDanhMuc)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/SaveDanhMucABRMasterData", aBRDanhMuc);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> DeleteDanhMucABRMasterData(int iD)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/DeleteDanhMucABRMasterData?iD=" + iD, iD);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region ABR Nhân vien
        public async Task<List<ABRNhanVien>> LayDanhSachABRNhanVien()
        {
            var result = await httpClient.GetFromJsonAsync<List<ABRNhanVien>>($"/api/ABR/getDanhSachABRNhanVien");
            return result;
        }

        public async Task<bool> SaveABRNhanVien(ABRNhanVien item)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/SaveABRNhanVien", item);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> DeleteABRNhanVien(Guid iD)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/DeleteABRNhanVien?iD=" + iD, iD);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> AddNhanVienHuongPool(Guid IDABRNhanVien, Guid IDABRPool)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["IDABRNhanVien"] = IDABRNhanVien.ToString(),
                ["IDABRPool"] = IDABRPool.ToString(),
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/AddNhanVienHuongPool", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, IDABRNhanVien);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteNhanVienHuongPool(Guid IDABRNhanVien, Guid IDABRPool)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["IDABRNhanVien"] = IDABRNhanVien.ToString(),
                ["IDABRPool"] = IDABRPool.ToString(),
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/DeleteNhanVienHuongPool", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, IDABRNhanVien);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<AbrPool>> GetNhanVienHuongPool(Guid IDABRNhanVien)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["IDABRNhanVien"] = IDABRNhanVien.ToString()
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetNhanVienHuongPool", queryStringParam);
            var result = await httpClient.GetFromJsonAsync<List<AbrPool>>(url);
            return result;
        }

        public async Task<List<ABRMapNhanVienABRHISVM>> GetDanhSachMapNhanVienABRHIS()
        {
            var result = await httpClient.GetFromJsonAsync<List<ABRMapNhanVienABRHISVM>>($"/api/ABR/GetDanhSachMapNhanVienABRHIS");
            return result;
        }

        public async Task<bool> SaveMapNhanVienABRHIS(ABRMapNhanVienABRHISVM item)
        {
            ABRMapNhanVienABRHIS rItem = new ABRMapNhanVienABRHIS()
            {
                ID = item.ID,
                IDNhanVienABR =  item.IDNhanVienABR,
                MaNhanVienHIS = item.MaNhanVienHIS
            };

            var response = await httpClient.PostAsJsonAsync($"/api/ABR/SaveMapNhanVienABRHIS", rItem);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteMapNhanVienABRHIS(Guid iD)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/DeleteMapNhanVienABRHIS?iD="+iD, iD);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<AbrNhanVienHisVM>> LayDanhSachNhanVienHIS()
        {
            var result = await httpClient.GetFromJsonAsync<List<AbrNhanVienHisVM>>($"/api/ABR/GetDanhSachNhanVienHIS");
            return result;
        }
        #endregion
        #region LuonDuocHuong
        public async Task<bool> AddNhanVienABRLuonDuocHuong(Guid IDABRNhanVien, int IDABRDanhMuc)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["IDABRNhanVien"] = IDABRNhanVien.ToString(),
                ["IDABRDanhMuc"] = IDABRDanhMuc.ToString(),
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/AddNhanVienABRLuonDuocHuong", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, IDABRNhanVien);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteNhanVienABRLuonDuocHuong(Guid IDABRNhanVien, int IDABRDanhMuc)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["IDABRNhanVien"] = IDABRNhanVien.ToString(),
                ["IDABRDanhMuc"] = IDABRDanhMuc.ToString(),
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/DeleteNhanVienABRLuonDuocHuong", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, IDABRNhanVien);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<ABRDanhMuc>> GetNhanVienABRLuonDuocHuong(Guid IDABRNhanVien)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["IDABRNhanVien"] = IDABRNhanVien.ToString()
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetNhanVienABRLuonDuocHuong", queryStringParam);
            var result = await httpClient.GetFromJsonAsync<List<ABRDanhMuc>>(url);
            return result;
        }

        #endregion
        #region HuongBacThang
        public async Task<ABRHuongBacThang> SaveHuongBacThang(ABRHuongBacThang aBRHuongBacThang)
        {
            string url = "/api/ABR/SaveHuongBacThang";
            var response = await httpClient.PostAsJsonAsync(url, aBRHuongBacThang);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ABRHuongBacThang>();
                return result;
            }
            else
            {
                return null;
            }
        }


        public async Task<bool> DeleteHuongBacThang(Guid ID)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["ID"] = ID.ToString(),              
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/DeleteHuongBacThang", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, ID);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<ABRHuongBacThang>> GetDanhSachHuongBacThang(int IDABRDanhMuc)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["IDABRDanhMuc"] = IDABRDanhMuc.ToString()
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetDanhSachHuongBacThang", queryStringParam);
            var result = await httpClient.GetFromJsonAsync<List<ABRHuongBacThang>>(url);    
            return result;
        }

        #endregion // Huong Bac Thang


        #region Pools
        public async Task<AbrPool> SavePool(AbrPool aBRPool)
        {
            string url = "/api/ABR/SavePool";
            var response = await httpClient.PostAsJsonAsync(url, aBRPool);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AbrPool>();
                return result;
            }
            else
            {
                return null;
            }
        }


        public async Task<bool> DeletePool(Guid ID)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["ID"] = ID.ToString(),
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/DeletePool", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, ID);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }
        public async Task<List<AbrPool>> GetDanhSachPool()
        {
            var result = await httpClient.GetFromJsonAsync<List<AbrPool>>($"/api/ABR/GetDanhSachPool");
            return result;

        }


        #endregion // Pools

        #region Nhân viên thực hiện
        public async Task<List<ABRNhanVienThucHienVM>> GetNhanVienThucHien(NhanVienThucHienSM nhanVienThucHienSM)
        {
            var result = new List<ABRNhanVienThucHienVM>();
            nhanVienThucHienSM.NumRecords = 5000;
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/getNhanVienThucHien", nhanVienThucHienSM);
            if (response.IsSuccessStatusCode)
            {

                var tempresult = await response.Content.ReadFromJsonAsync<List<ABRNhanVienThucHienVM>>();
                while (tempresult.Count>0)
                {
                    result.AddRange(tempresult);
                    nhanVienThucHienSM.Trang++;
                    response = await httpClient.PostAsJsonAsync($"/api/ABR/getNhanVienThucHien", nhanVienThucHienSM);
                    if (response.IsSuccessStatusCode)
                    {
                        tempresult = await response.Content.ReadFromJsonAsync<List<ABRNhanVienThucHienVM>>();
                    }
                    else
                    {
                        Console.WriteLine(response.ToString());
                        break;
                    }
                }
                return result;
            }
            else
            {
                Console.WriteLine("GetNhanVienThucHien" + response.Content);
                return null;
            }
        }

        public async Task<bool> SaveABRNhanVienThucHien(ABRNhanVienThucHien aBRNhanVienThucHien)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/SaveNhanVienThucHien", aBRNhanVienThucHien);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> DeleteABRNhanVienThucHien(ABRNhanVienThucHien aBRNhanVienThucHien)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/DeleteNhanVienThucHien", aBRNhanVienThucHien);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region Báo cáo
        public async Task<IEnumerable<IDictionary<string, object>>> GetBaoCaoChiTietDaThucHien(NhanVienThucHienSM nhanVienThucHienSM)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/GetBaoCaoChiTietDaThucHien", nhanVienThucHienSM);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<IDictionary<string, object>>>();
                return result.AsODataEnumerable();
            }
            else
            {
                return null;
            }
        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetBaoDaThucHien(NhanVienThucHienSM nhanVienThucHienSM)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/GetBaoCaoDaThucHien", nhanVienThucHienSM);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<IDictionary<string, object>>>();
                return result.AsODataEnumerable();
            }
            else
            {
                return null;
            }
        }
        
        public async Task<List<ABRDieuChinh>> GetABRDieuChinh(int thang, int nam)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = thang.ToString(),
                ["nam"] = nam.ToString(),

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetABRDieuChinh", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<ABRDieuChinh>>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> SaveABRDieuChinh(ABRDieuChinh item)
        {   
            var response = await httpClient.PostAsJsonAsync("/api/ABR/SaveABRDieuChinh", item);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }
        public async Task<List<AbrSoSanhChiTietVM>> GetABRSoSanhChiTiet(int thang, int nam, string MaBenhVienChiNhanh)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = thang.ToString(),
                ["nam"] = nam.ToString(),
                ["MaBenhVienChiNhanh"] = MaBenhVienChiNhanh.ToString(),

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetABRSoSanhChiTiet", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<AbrSoSanhChiTietVM>>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ABRSoSanhTongHopVM>> GetABRSoSanhTongHop(int thang, int nam, string MaBenhVienChiNhanh)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = thang.ToString(),
                ["nam"] = nam.ToString(),
                ["MaBenhVienChiNhanh"] = MaBenhVienChiNhanh.ToString()

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetABRSoSanhTongHop", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<ABRSoSanhTongHopVM>>();
                return result;
            }
            else
            {
                return null;
            }
        }

        
        public async Task<List<ABRSoSanhTongHopVM>> GetABRDoanhThuThangTongHop(int thang, int nam, string MaBenhVienChiNhanh)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = thang.ToString(),
                ["nam"] = nam.ToString(),
                ["MaBenhVienChiNhanh"] = MaBenhVienChiNhanh.ToString(),

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetABRDoanhThuThangTongHop", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<ABRSoSanhTongHopVM>>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ABRSoSanhTongHopVM>> GetABRDieuChinhThangTongHop(int thang, int nam, string MaBenhVienChiNhanh)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = thang.ToString(),
                ["nam"] = nam.ToString(),
                ["MaBenhVienChiNhanh"] = MaBenhVienChiNhanh.ToString(),

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetABRDieuChinhThangTongHop", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<ABRSoSanhTongHopVM>>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ABRDanhGiaNhanVien>> GetBangDanhGiaNhanVien(int thang, int nam)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = thang.ToString(),
                ["nam"] = nam.ToString(),

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetBangDanhGiaNhanVien", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<ABRDanhGiaNhanVien>>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<IDictionary<string, object>>> GetLuongChiTietDichVu(int thang, int nam)
        {

            int page = 0;
            int NumRecords = 500;

            List<IDictionary<string, object>> result = new List<IDictionary<string, object>>();

            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = thang.ToString(),
                ["nam"] = nam.ToString(),
                ["page"] = page.ToString(),
                ["NumRecords"] = NumRecords.ToString()

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetLuongChiTietDichVu", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var resulttam = await response.Content.ReadFromJsonAsync<List<IDictionary<string, object>>>();
                while (resulttam.Count > 0)
                {
                    result.AddRange(resulttam);
                    page++;
                    queryStringParam = new Dictionary<string, string>
                    {
                        ["thang"] = thang.ToString(),
                        ["nam"] = nam.ToString(),
                        ["page"] = page.ToString(),
                        ["NumRecords"] = NumRecords.ToString()

                    };
                    url = QueryHelpers.AddQueryString("/api/ABR/GetLuongChiTietDichVu", queryStringParam);
                    response = await httpClient.PostAsJsonAsync(url, queryStringParam);
                    if (response.IsSuccessStatusCode)
                    { 
                        resulttam = await response.Content.ReadFromJsonAsync<List<IDictionary<string, object>>>();
                    }
                    else
                    {
                        break;
                    }
                }                
                return result;
            }
            else
            {
                return null;
            }
        }

        
        public async Task<IEnumerable<IDictionary<string, object>>> GetLuongNhomDichVu(int thang, int nam)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = thang.ToString(),
                ["nam"] = nam.ToString(),

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetGetLuongNhomDichVu", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<IDictionary<string, object>>>();
                return result;
            }
            else
            {
                return null;
            }
        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetNhanVienThucHienHis(NhanVienThucHienSM nhanVienThucHienSM)
        {           
            
            var response = await httpClient.PostAsJsonAsync("/api/ABR/GetNhanVienThucHienHis", nhanVienThucHienSM);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<IDictionary<string, object>>>();
                return result;
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region Tính ABR
        public async Task<bool> ABRSaveDanhGiaNhanVien(ABRDanhGiaNhanVien aBRDanhGiaNhanVien)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/ABR/SaveDanhGiaNhanVien", aBRDanhGiaNhanVien);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteBangDanhGiaNhanVien(int thang, int nam)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = thang.ToString(),
                ["nam"] = nam.ToString(),

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/DeleteBangDanhGiaNhanVien", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<ABRChiTietNhanVienVM>> GetABRChiTietNhanVien(string MaNhanVien, int thang, int nam)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["MaNhanVien"] = MaNhanVien,
                ["thang"] = thang.ToString(),
                ["nam"] = nam.ToString(),

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetABRChiTietNhanVien", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<ABRChiTietNhanVienVM>>();
                return result;
            }
            else
            {
                return null;
            }
        }       

        public async Task<bool> SaveXetDuyet(int Thang, int Nam, int Muc, List<ABRSoSanhTongHopVM> listTongHop)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = Thang.ToString(),
                ["nam"] = Nam.ToString(),
                ["muc"] = Muc.ToString(),

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/SaveXetDuyet", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, listTongHop);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }

        }
        public async Task<bool> DeleteXetDuyet(int Thang, int Nam, int Muc, string MaBenhVienChiNhanh="")
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = Thang.ToString(),
                ["nam"] = Nam.ToString(),
                ["muc"] = Muc.ToString(),
                ["MaBenhVienChiNhanh"] = MaBenhVienChiNhanh

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/DeleteXetDuyet", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }

        }
        public async Task<bool> CheckDaTinhSoLuongABR(int Thang, int Nam)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = Thang.ToString(),
                ["nam"] = Nam.ToString(),

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/CheckDaTinhSoLuongABR", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckDaUploadBangDanhGia(int Thang, int Nam)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = Thang.ToString(),
                ["nam"] = Nam.ToString(),

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/CheckDaUploadBangDanhGia", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckDaTinhABR(int Thang, int Nam)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = Thang.ToString(),
                ["nam"] = Nam.ToString(),

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/CheckDaTinhABR", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> CheckDaXetDuyet(int Thang, int Nam, int Muc, string MaBenhVienChiNhanh)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = Thang.ToString(),
                ["nam"] = Nam.ToString(),
                ["Muc"] = Muc.ToString(),
                ["MaBenhVienChiNhanh"] = MaBenhVienChiNhanh
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/CheckDaXetDuyet", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> CheckDaXetDuyetTheoNgay(DateTime Ngay, string MaBenhVienChiNhanh="")
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["Ngay"] = Ngay.ToString("s"),
                ["MaBenhVienChiNhanh"] = MaBenhVienChiNhanh
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/CheckDaXetDuyetTheongay", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url,Ngay.ToString("s"));
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> GetSoLanTuChoi(int Thang, int Nam, int Muc)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = Thang.ToString(),
                ["nam"] = Nam.ToString(),
                ["Muc"] = Muc.ToString(),
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetSoLanTuChoi", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<int>();
                return result;
            }
            else
            {
                return 0;
            }
        }
        public async Task<bool> TinhHuongABR(int Thang, int Nam)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = Thang.ToString(),
                ["nam"] = Nam.ToString(),

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/TinhHuongABR", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<ABRNganSachThang>> GetNganSachThang(int thang, int nam, string MaBenhVienChiNhanh="")
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["thang"] = thang.ToString(),
                ["nam"] = nam.ToString(),
                ["MaBenhVienChiNhan"] = MaBenhVienChiNhanh,

            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetNganSachThang", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<ABRNganSachThang>>();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> SaveNganSachThang(ABRNganSachThang item)
        {
            
            var response = await httpClient.PostAsJsonAsync("/api/ABR/SaveNganSachThang", item);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }
        public async Task<List<ABRThucHienCuoiVM>> GetThucHienCuoi()
        {
            var result = await httpClient.GetFromJsonAsync<List<ABRThucHienCuoiVM>>($"/api/ABR/GetThucHienCuoi");
            return result;
        }

        #endregion

        #region Loại vai trò
        public async Task<List<ABRLoaiVaiTro>> GetDanhSachLoaiVaiTro()
        {
            
            try
            {
                var result = await httpClient.GetFromJsonAsync<List<ABRLoaiVaiTro>>($"/api/ABR/GetDanhSachLoaiVaiTro");
                return result;
            }
            catch
            {

            }
            return null;
        }
        #endregion //Loại vai trò

        #region Pool theo danh mục
        public async Task<List<AbrPool>> GetPoolHuongTheoDanhMuc(int idDanhMuc)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["IdDanhMuc"] = idDanhMuc.ToString(),
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/GetPoolHuongTheoDanhMuc", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, idDanhMuc);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<AbrPool>>();
                return result;
            }
            else
            {
                return new List<AbrPool>();
            }
        }
        public async Task<bool> DeletePoolHuongTheoDanhMuc(int idDanhMuc)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["IdDanhMuc"] = idDanhMuc.ToString(),
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/DeletePoolHuongTheoDanhMuc", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, idDanhMuc);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SavePoolHuongTheoDanhMuc(int idDanhMuc, Guid idPool)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["IdDanhMuc"] = idDanhMuc.ToString(),
                ["IdPool"] = idPool.ToString(),
            };
            string url = QueryHelpers.AddQueryString("/api/ABR/SavePoolHuongTheoDanhMuc", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, idDanhMuc);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }
            else
            {
                return false;
            }
        }
  
        #endregion

    #region Tháng Năm
    public async Task<ABRThangNam> GetNgayTheoThang(int Thang, int Nam)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["Thang"] = Thang.ToString(),
                ["Nam"] = Nam.ToString()
            };
            string url = QueryHelpers.AddQueryString("api/ABR/GetNgayTheoThang", queryStringParam);
            var result = await httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var orderRe = JsonSerializer.Deserialize<ABRThangNam>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return orderRe;
            }
            return null;
        }
        public async Task<ABRThangNam> SaveNgayTheoThang(ABRThangNam item)
        {            
            
            var response = await httpClient.PostAsJsonAsync("api/ABR/SaveNgayTheoThang", item);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ABRThangNam>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Mức tính ABR cho nhân viên
        public async Task<List<ABRDanhMucNhanVienVM>> GetDanhSackDanhMucNhanVien(Guid IDNhanVien)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["IDNhanVien"] = IDNhanVien.ToString(),               
            };
            string url = QueryHelpers.AddQueryString("api/ABR/GetDanhSackDanhMucNhanVien", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, IDNhanVien);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<ABRDanhMucNhanVienVM>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            return null;
        }

        public async Task<ABRDanhMucNhanVienVM> SaveDanhMucNhanVien( ABRDanhMucNhanVienVM aBRDanhMucNhanVienVM)
        {           
            var response = await httpClient.PostAsJsonAsync("api/ABR/SaveDanhMucNhanVien", aBRDanhMucNhanVienVM);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ABRDanhMucNhanVienVM>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            return null;
        }
        public async Task<bool> DeleteDanhMucNhanVien(ABRDanhMucNhanVienVM aBRDanhMucNhanVienVM)
        {
            var response = await httpClient.PostAsJsonAsync("api/ABR/DeleteDanhMucNhanVien", aBRDanhMucNhanVienVM);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<bool>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            return false;
        }
        #endregion
        #region Xác nhận his
        public async Task<List<ABRXacNhanNhanVienThucHienHisVM>> GetXacNhanNhanVienThucHienHis(DateTime TuNgay, DateTime DenNgay)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["Para"] = authService.GetUserID().ToString(),
                ["TuNgay"] = TuNgay.ToString(),
                ["DenNgay"] = DenNgay.ToString()
            };            
            string url = QueryHelpers.AddQueryString("api/ABR/GetXacNhanNhanVienThucHienHis", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, new TuNgayDenNgayOneParaSM() { Para1= (await authService.GetUserID()).ToString(), TuNgay = TuNgay, DenNgay = DenNgay });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<ABRXacNhanNhanVienThucHienHisVM>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            return null;
        }
        public async Task<ABRXacNhanNhanVienThucHienHisVM> SaveXacNhanNhanVienThucHienHis(ABRXacNhanNhanVienThucHienHisVM item)
        {
            try
            {
                var queryStringParam = new Dictionary<string, string>
                {
                    ["item"] = item.ToString()
                };
                item.UserLuu = await authService.GetUserID();
                string url = QueryHelpers.AddQueryString("api/ABR/SaveXacNhanNhanVienThucHienHis", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, item);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {                    
                    var result = JsonSerializer.Deserialize<ABRXacNhanNhanVienThucHienHisVM>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    return result;
                }                
                Console.WriteLine( content );
            } catch(Exception ex)
            {
                Console.WriteLine( ex.Message.ToString() );
            }
            return null;
        }
        public async Task<bool> DeleteXacNhanNhanVienThucHienHis(ABRXacNhanNhanVienThucHienHisVM item)
        {
            try
            {
                var queryStringParam = new Dictionary<string, string>
                {
                    ["item"] = item.ToString()
                };
                string url = QueryHelpers.AddQueryString("api/ABR/DeleteXacNhanNhanVienThucHienHis", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, item);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {                    
                    return true;
                }
                Console.WriteLine(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            return false;
        }
        public async Task<bool> DeleteUserKetThucCongViecHis(Guid ID)
        {
            try
            {
                var queryStringParam = new Dictionary<string, string>
                {
                    ["ID"] = ID.ToString()
                };
                string url = QueryHelpers.AddQueryString("api/ABR/DeleteUserKetThucCongViecHis", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, ID);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<bool>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    return result;
                }
                Console.WriteLine(response.ToString());
                return false;
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }            
        }

        public async Task<ABRUserKetThucCongViecHis> SaveUserKetThucCongViecHis(ABRUserKetThucCongViecHis item)
        {
            try
            {
                var queryStringParam = new Dictionary<string, string>
                {
                    ["item"] = item.ToString()
                };
                string url = QueryHelpers.AddQueryString("api/ABR/SaveUserKetThucCongViecHis", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, item);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ABRUserKetThucCongViecHis>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    return result;
                }
                Console.WriteLine(response.ToString());
                return null;
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }
        }
        public async Task<List<ABRUserKetThucCongViecHisVM>> GetUserKetThucCongViecHis()
        {
            try
            {
                var queryStringParam = new Dictionary<string, string>
                {                    
                };
                string url = QueryHelpers.AddQueryString("api/ABR/GetUserKetThucCongViecHis", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<ABRUserKetThucCongViecHisVM>>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    return result;
                }
                Console.WriteLine(response.ToString());
                return new List<ABRUserKetThucCongViecHisVM>();
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return new List<ABRUserKetThucCongViecHisVM>();
            }
        }

        public async Task<List<UserVM>> LayUserKetThucHis()
        {
            try
            {
                Console.WriteLine("LayUserKetThucHis 1");
                var queryStringParam = new Dictionary<string, string>
                {                    
                };
                string url = QueryHelpers.AddQueryString("api/ABR/LayUserKetThucHis", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
                Console.WriteLine("LayUserKetThucHis 2");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(response.ToString());
                    Console.WriteLine(content.ToString());
                    var result = JsonSerializer.Deserialize<List<UserVM>>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    Console.WriteLine("result:" + result.ToString());
                    return result;
                }
                Console.WriteLine(response.ToString());
                return new List<UserVM>();
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return new List<UserVM>();
            }
        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetBaoCaoXacNhanNhanVienHis(DateTime TuNgay, DateTime DenNgay)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["TuNgay"] = TuNgay.ToString(),
                ["DenNgay"] = DenNgay.ToString(),
            };
            TuNgayDenNgayOneParaSM item = new TuNgayDenNgayOneParaSM() { Para1 = (await authService.GetUserID()).ToString(), TuNgay = TuNgay, DenNgay = DenNgay };


            string url = QueryHelpers.AddQueryString("api/ABR/GetBaoCaoXacNhanNhanVienHis", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, item);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<IEnumerable<IDictionary<string, object>>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            return null;
        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetBaoCaoChiTietXacNhanNhanVienHis(DateTime TuNgay, DateTime DenNgay)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["TuNgay"] = TuNgay.ToString(),
                ["DenNgay"] = DenNgay.ToString(),
            };
            TuNgayDenNgayOneParaSM item = new TuNgayDenNgayOneParaSM() { Para1 = (await authService.GetUserID()).ToString(), TuNgay = TuNgay, DenNgay = DenNgay };


            string url = QueryHelpers.AddQueryString("api/ABR/GetBaoCaoChiTietXacNhanNhanVienHis", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, item);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<IEnumerable<IDictionary<string, object>>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            return null;
        }

        public async Task<List<ABRUserXacNhanNoiLamViec>> GetUserXacNhanNoiLamViecThucHien()
        {
            try
            {                
                var queryStringParam = new Dictionary<string, string>
                {
                };
                string url = QueryHelpers.AddQueryString("api/ABR/GetUserXacNhanNoiLamViecThucHien", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, queryStringParam);                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(response.ToString());
                    Console.WriteLine(content.ToString());
                    var result = JsonSerializer.Deserialize<List<ABRUserXacNhanNoiLamViec>>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    Console.WriteLine("result:" + result.ToString());
                    return result;
                }
                Console.WriteLine(response.ToString());
                return new List<ABRUserXacNhanNoiLamViec>();
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return new List<ABRUserXacNhanNoiLamViec>();
            }
        }
        public async Task<ABRUserXacNhanNoiLamViec> SaveUserXacNhanNoiLamViecThucHien(ABRUserXacNhanNoiLamViec aBRUserXacNhanNoiLamViec)
        {
            try
            {
                var queryStringParam = new Dictionary<string, string>
                {
                };
                string url = QueryHelpers.AddQueryString("api/ABR/SaveUserXacNhanNoiLamViecThucHien", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, aBRUserXacNhanNoiLamViec);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(response.ToString());
                    Console.WriteLine(content.ToString());
                    var result = JsonSerializer.Deserialize<ABRUserXacNhanNoiLamViec>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    Console.WriteLine("result:" + result.ToString());
                    return result;
                }
                Console.WriteLine(response.ToString());
                return null;
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }

        }
        public async Task<bool> DeleteUserXacNhanNoiLamViecThucHien(ABRUserXacNhanNoiLamViec aBRUserXacNhanNoiLamViec)
        {
            try
            {
                var queryStringParam = new Dictionary<string, string>
                {
                };
                string url = QueryHelpers.AddQueryString("api/ABR/DeleteUserXacNhanNoiLamViecThucHien", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, aBRUserXacNhanNoiLamViec);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(response.ToString());
                    Console.WriteLine(content.ToString());
                    var result = JsonSerializer.Deserialize<bool>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    Console.WriteLine("result:" + result.ToString());
                    return result;
                }
                Console.WriteLine(response.ToString());
                return false;
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }
        public async Task<List<ABRUserXacNhanNoiLamViec>> GetUserXacNhanNoiLamViecChiDinh()
        {
            try
            {
                var queryStringParam = new Dictionary<string, string>
                {
                };
                string url = QueryHelpers.AddQueryString("api/ABR/GetUserXacNhanNoiLamViecChiDinh", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(response.ToString());
                    Console.WriteLine(content.ToString());
                    var result = JsonSerializer.Deserialize<List<ABRUserXacNhanNoiLamViec>>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    Console.WriteLine("result:" + result.ToString());
                    return result;
                }
                Console.WriteLine(response.ToString());
                return new List<ABRUserXacNhanNoiLamViec>();
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return new List<ABRUserXacNhanNoiLamViec>();   
            }
        }
        public async Task<ABRUserXacNhanNoiLamViec> SaveUserXacNhanNoiLamViecChiDinh(ABRUserXacNhanNoiLamViec aBRUserXacNhanNoiLamViec)
        {
            try
            {
                var queryStringParam = new Dictionary<string, string>
                {
                };
                string url = QueryHelpers.AddQueryString("api/ABR/SaveUserXacNhanNoiLamViecChiDinh", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, aBRUserXacNhanNoiLamViec);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(response.ToString());
                    Console.WriteLine(content.ToString());
                    var result = JsonSerializer.Deserialize<ABRUserXacNhanNoiLamViec>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    Console.WriteLine("result:" + result.ToString());
                    return result;
                }
                Console.WriteLine(response.ToString());
                return null;
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }

        }
        public async Task<bool> DeleteUserXacNhanNoiLamViecChiDinh(ABRUserXacNhanNoiLamViec aBRUserXacNhanNoiLamViec)
        {
            try
            {
                var queryStringParam = new Dictionary<string, string>
                {
                };
                string url = QueryHelpers.AddQueryString("api/ABR/DeleteUserXacNhanNoiLamViecChiDinh", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, aBRUserXacNhanNoiLamViec);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(response.ToString());
                    Console.WriteLine(content.ToString());
                    var result = JsonSerializer.Deserialize<bool>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    Console.WriteLine("result:" + result.ToString());
                    return result;
                }
                Console.WriteLine(response.ToString());
                return false;
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }
        public async Task<List<ABRNoiLamViecVM>> GetNoiLamViec()
        {
            try
            {
                var queryStringParam = new Dictionary<string, string>
                {
                };
                string url = QueryHelpers.AddQueryString("api/ABR/GetNoiLamViec", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, queryStringParam);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(response.ToString());
                    Console.WriteLine(content.ToString());
                    var result = JsonSerializer.Deserialize<List<ABRNoiLamViecVM>>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    Console.WriteLine("result:" + result.ToString());
                    return result;
                }
                Console.WriteLine(response.ToString());
                return new List<ABRNoiLamViecVM>();
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return new List<ABRNoiLamViecVM>();
            }
        }
        #endregion

        #region ngày công nhân viên
        public async Task<List<ABRNgayCong>> GetNgayCong(int Thang, int Nam)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["Thang"] = Thang.ToString(),
                ["Nam"] = Nam.ToString(),                
            };
            string url = QueryHelpers.AddQueryString("api/ABR/GetNgayCong", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url,Thang);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<ABRNgayCong>>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            return null;
        }
        public async Task<ABRNgayCong> SaveNgayCong(ABRNgayCong item)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["item"] = item.ToString(),                
            };
            string url = QueryHelpers.AddQueryString("api/ABR/SaveNgayCong", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, item);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ABRNgayCong>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            return null;
        }

        public async Task<bool> DeleteNgayCong(ABRNgayCong item)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["item"] = item.ToString(),
            };
            string url = QueryHelpers.AddQueryString("api/ABR/DeleteNgayCong", queryStringParam);
            var response = await httpClient.PostAsJsonAsync(url, item);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<bool>(content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                    );
                return result;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            return false;
        }
        #endregion
        #region Nhiều bệnh viện        
        public async Task<List<BenhVienVM>> GetBenhVien()
        {
            try
            {
                var queryStringParam = new Dictionary<string, string>
                {
                };
                string url = QueryHelpers.AddQueryString("api/ABR/GetBenhVien", queryStringParam);
                var response = await httpClient.PostAsJsonAsync(url, new { });
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<BenhVienVM>>(content,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    return result;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                }
            }
            catch( Exception ex )
            {
                Console.WriteLine(ex.Message);
            }
            return new List<BenhVienVM>();
        }

        #endregion
    }
}
