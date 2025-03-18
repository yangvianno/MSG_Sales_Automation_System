using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using HelperLib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using SalesAuto.Models.ViewModel;
using SalesAuto.Wasm.Services;
using SalesAuto.Wasm.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Pages
{
    public partial class ReportDownload
    {
        [Inject] private HttpClient httpClient { get; set; }
        [Inject] IToastService toastService { get; set; }
        [Inject] private IJSRuntime jsRuntime { get; set; }
        [Inject] IBenhVienClient benhVienClient { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }
        private List<TuanVM> TuanVMs { get; set; }
        private int tuan = 0;
        private int nam = 0;
        private int thang = 0;
        private string MaBenhVien = "O";
        private string ToMail = "thanh.tran@matsaigon.com";
        protected override async Task OnInitializedAsync()
        {
            thang = DateTime.Now.Month;
            nam = DateTime.Now.Year;
            if (thang == 1)
            {
                thang = 12;
                nam--;
            }
            else
            {
                thang--;
            }
            CultureInfo myCI = new CultureInfo("vi-VN");
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            Calendar myCal = myCI.Calendar;
            tuan = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW);
            tuan--;
            TuanVMs = DateTimeHelp.LayTuanTrongNam(nam);
            MaBenhVien = await benhVienClient.getBenhVienDangLamViec();
        }
        public Task loadDanhMucThang()
        {
            TuanVMs = DateTimeHelp.LayTuanTrongNam(nam);
            return Task.CompletedTask;
        }

        public async Task DownLoadPCAExcel()
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", $"Confirm download?"))
            {
                var queryStringParam = new Dictionary<string, string>
                {
                    ["nam"] = nam.ToString(),
                    ["thang"] = thang.ToString()
                };

                string url = QueryHelpers.AddQueryString("api/ReportExcel/CPA", queryStringParam);
                var fileName = $"PCAFile.xlsx";
                await DownloadFile(url, fileName);
            }
        }
        public async Task SendMailPCAExcel()
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["nam"] = nam.ToString(),
                ["thang"] = thang.ToString()
            };
            string url = QueryHelpers.AddQueryString("api/ReportExcel/SendMailCPA", queryStringParam);
            await GuiMail(url);
        }
        public async Task TaoDuLieuPCA()
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["nam"] = nam.ToString(),
                ["thang"] = thang.ToString()
            };
            string url = QueryHelpers.AddQueryString("api/ReportExcel/TaoDuLieuCPA", queryStringParam);
            await TaoDuLieu(url);
        }
        public async Task DownLoadLeadFollowExcel()
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", $"Confirm download?"))
            {
                var queryStringParam = new Dictionary<string, string>
                {
                    ["nam"] = nam.ToString(),
                    ["thang"] = thang.ToString()
                };

                string url = QueryHelpers.AddQueryString("api/ReportExcel/leadfollow", queryStringParam);
                var fileName = $"LeadFollowFile.xlsx";
                await DownloadFile(url, fileName);
            }
        }
        public async Task SendMailLeadFollowExcel()
        {

            var queryStringParam = new Dictionary<string, string>
            {
                ["nam"] = nam.ToString(),
                ["thang"] = thang.ToString()
            };
            string url = QueryHelpers.AddQueryString("api/ReportExcel/SendMailleadfollow", queryStringParam);
            await GuiMail(url);
        }
        public async Task TaoDuLieuLeadFollow()
        {

            var queryStringParam = new Dictionary<string, string>
            {
                ["nam"] = nam.ToString(),
                ["thang"] = thang.ToString()
            };
            string url = QueryHelpers.AddQueryString("api/ReportExcel/TaoDuLieuleadfollow", queryStringParam);
            await TaoDuLieu(url);
        }

        private async Task GuiMail(string url)
        {
            string message = "Dữ liệu báo cáo cho tới tháng " + thang + "-" + nam + " sẽ không thể thay đổi. Bạn chắc chắn muốn gửi mail?";
            var parameters = new ModalParameters();
            parameters.Add(nameof(Confirm.Message), message);
            var moviesModal = Modal.Show<Confirm>("Confirm", parameters);
            var result = await moviesModal.Result;

            if (result.Cancelled)
            {

            }
            else
            {
                try
                {
                    toastService.ShowInfo("Đang gửi mail");
                    var response = await httpClient.PostAsJsonAsync(url, nam);
                    if (response.IsSuccessStatusCode)
                    {
                        toastService.ShowSuccess("Gửi mail hoàn tất");
                    }
                    else
                    {
                        toastService.ShowError("Gửi mail bị lỗi " + response.RequestMessage.ToString());
                    }
                }
                catch (Exception ex)
                {
                    toastService.ShowError("Gửi mail bị lỗi " + ex.Message);
                }

            }

        }
        private async Task TaoDuLieu(string url)
        {
            string message = "Các dữ liệu trong tháng " + thang + "-" + nam + " sẽ được xóa và tạo lại. Bạn chắc chắn muốn thực hiện?";
            var parameters = new ModalParameters();
            parameters.Add(nameof(Confirm.Message), message);
            var moviesModal = Modal.Show<Confirm>("Confirm", parameters);
            var result = await moviesModal.Result;

            if (result.Cancelled)
            {

            }
            else
            {
                try
                {
                    toastService.ShowInfo("Đang xóa và tạo lại!");
                    var response = await httpClient.PostAsJsonAsync(url, nam);
                    if (response.IsSuccessStatusCode)
                    {
                        toastService.ShowSuccess("Hoàn tất!");
                    }
                    else
                    {
                        toastService.ShowError("Bị lỗi " + response.RequestMessage.ToString());
                    }
                }
                catch (Exception ex)
                {
                    toastService.ShowError("Bị lỗi " + ex.Message);
                }

            }

        }

        public async Task SendMailKPIExcel()
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["nam"] = nam.ToString(),
                ["thang"] = thang.ToString()
            };
            string url = QueryHelpers.AddQueryString("api/ReportExcel/SendMailkpi", queryStringParam);
            await GuiMail(url);

        }
        public async Task DownLoadKPIExcel()
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", $"Confirm download?"))
            {
                var queryStringParam = new Dictionary<string, string>
                {
                    ["nam"] = nam.ToString(),
                    ["thang"] = thang.ToString()
                };

                string url = QueryHelpers.AddQueryString("api/ReportExcel/kpi", queryStringParam);
                var fileName = $"KPIFile.xlsx";
                await DownloadFile(url, fileName);
            }
        }

        public async Task SendMailLeadsChannelExcel()
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["nam"] = nam.ToString(),
                ["thang"] = thang.ToString()
            };
            string url = QueryHelpers.AddQueryString("api/ReportExcel/SendMailLeadsChannel", queryStringParam);
            await GuiMail(url);

        }
        public async Task DownLoadLeadsChannelExcel()
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", $"Confirm download?"))
            {
                var queryStringParam = new Dictionary<string, string>
                {
                    ["nam"] = nam.ToString(),
                    ["thang"] = thang.ToString()
                };

                string url = QueryHelpers.AddQueryString("api/ReportExcel/LeadsChannel", queryStringParam);
                var fileName = $"LeadsChannelFile.xlsx";
                await DownloadFile(url, fileName);
            }
        }

        private async Task DownloadFile(string url, string fileName)
        {
            toastService.ShowInfo("Đang load file ...");
            var response = await httpClient.PostAsJsonAsync(url, nam);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                await jsRuntime.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(fileBytes));
                toastService.ShowSuccess("Lưu file hoàn tất!");
            }
            else
            {
                toastService.ShowSuccess("Lỗi!" + response.RequestMessage.ToString());
            }
        }

        public async Task DownloadAll()
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", $"Confirm download?"))
            {
                var queryStringParam = new Dictionary<string, string>
                {
                    ["nam"] = nam.ToString(),
                    ["thang"] = thang.ToString()
                };

                string url = QueryHelpers.AddQueryString("api/ReportExcel/DownloadAll", queryStringParam);
                //var response = await httpClient.PostAsJsonAsync(url, nam);
                //response.EnsureSuccessStatusCode();
                //var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = $"CPACallCenterFile.xlsx";
                await DownloadFile(url, fileName);
                //await jsRuntime.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(fileBytes));
            }
        }
        public async Task SendMailAll()
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["nam"] = nam.ToString(),
                ["thang"] = thang.ToString()
            };

            string url = QueryHelpers.AddQueryString("api/ReportExcel/SendMailAll", queryStringParam);

            await GuiMail(url);
        }

        // Bao Cao tu Dong
        public async Task DownLoadBaoCaoThang()
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", $"Confirm download?"))
            {
                var queryStringParam = new Dictionary<string, string>
                {
                    ["nam"] = nam.ToString(),
                    ["thang"] = thang.ToString()
                };

                string url = QueryHelpers.AddQueryString("api/ReportExcel/BaoCaoThang", queryStringParam);
                var fileName = $"{nam}{thang}MonntlyReport.xlsx";
                await DownloadFile(url, fileName);
            }
        }

        public async Task DownLoadBaoCaoTuan()
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", $"Confirm download?"))
            {
                var queryStringParam = new Dictionary<string, string>
                {
                    ["nam"] = nam.ToString(),
                    ["tuan"] = tuan.ToString()
                };
                string url = QueryHelpers.AddQueryString("api/ReportExcel/BaoCaoTuan", queryStringParam);
                var fileName = $"{nam}{tuan}WeeklyReport.xlsx";
                await DownloadFile(url, fileName);
            }
        }

        public async Task SendMailBaoCaoTuan()
        {
            toastService.ShowInfo("Đang gửi mail ...");
            var queryStringParam = new Dictionary<string, string>
            {
                ["nam"] = nam.ToString(),
                ["tuan"] = tuan.ToString(),
                ["resend"] = true.ToString(),
                ["ToMail"] = ToMail

            };
            string url = QueryHelpers.AddQueryString("api/ReportExcel/SendMailBaoCaoTuan", queryStringParam);
            var rs = await httpClient.PostAsJsonAsync(url, ToMail);                                    
            if (rs.IsSuccessStatusCode)
            {                
                toastService.ShowSuccess("Gửi mail hoàn tất!");
            }
            else
            {
                toastService.ShowSuccess("Lỗi!" + rs.RequestMessage.ToString());
            }

        }
    }
}

