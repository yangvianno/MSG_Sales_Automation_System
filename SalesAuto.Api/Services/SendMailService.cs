using DataAccessLibrary;
using DB;
using HelperLib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SalesAuto.Api.Repositories;
using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SalesAuto.Api.Services
{
    public class SendMailService: IHostedService, IDisposable
    {
        private readonly ILogger<SendMailService> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration config;
        private readonly IOptions<MailSettings> mailSettings;
        private readonly IHttpClientFactory client_HttpClientFactory;
        private ISaleAutoReportRepo reportRepository;
        private IDailyReportRepo dailyReportRepository;
        private IMailRepo mailRepo;
        private IBenhNhansRepo benhNhansRepository;
        private SalesAutoDbContext context;
        private SqlDataAccess sqlDataAccess;
        private Timer _timerLayLeads;
        private Timer _timerPushCRM;

        private ICRMClientRepo cRMClientRep;
        private IHenKhamRepo henKhamRepo;
        private IBenhViensRepo benhViensRepository;
        public SendMailService(ILogger<SendMailService> logger, IServiceProvider serviceProvider, IConfiguration config, IOptions<MailSettings> mailSettings, IHttpClientFactory client_httpClientFactory)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.config = config;
            this.mailSettings = mailSettings;
            client_HttpClientFactory = client_httpClientFactory;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation(DateTime.Now.ToString() + "Send mail is starting.");
            LogHelp.Write(DateTime.Now.ToString() + "Send mail is starting.");
            context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<SalesAutoDbContext>();
            sqlDataAccess = new SqlDataAccess(config);
            benhNhansRepository = new BenhNhansRepo(context, sqlDataAccess);
            reportRepository = new SaleAutoReportRepo(benhNhansRepository, context, sqlDataAccess);
            mailRepo = new MailRepo(mailSettings);
            dailyReportRepository = new DailyReportRepo(sqlDataAccess);
            cRMClientRep = new CRMClientRepo(config, client_HttpClientFactory, context, sqlDataAccess);
            benhViensRepository = new BenhViensRepo(context, sqlDataAccess);
            henKhamRepo = new HenKhamRepo(context, sqlDataAccess, benhViensRepository, cRMClientRep); 

            TimeSpan interval = TimeSpan.FromMinutes(30);           

            Action action = () =>
            {   
                SendMailLeads(null);                
                var t1 = Task.Delay(interval);
                t1.Wait();                
                _timerLayLeads = new Timer(
                    SendMailLeads,
                    null,
                    TimeSpan.Zero,
                    interval
                );
            };            
            Task.Run(action);

            TimeSpan intervalPushHenKham = TimeSpan.FromMinutes(15);
            Action actionPushHenKham = () =>
            {   
                PushHenKham(null);
                var t1 = Task.Delay(intervalPushHenKham);
                t1.Wait();

                _timerPushCRM = new Timer(
                    PushHenKham,
                    null,
                    TimeSpan.Zero,
                    intervalPushHenKham
                );
            };
            Task.Run(actionPushHenKham);

            return Task.CompletedTask;
        }

        bool isRunning = false;

        bool isPushingHenKham = false;
        private async void PushHenKham(object state)
        {
            if (isPushingHenKham)
                return;
            isPushingHenKham = true;

            try
            {
                logger.LogInformation(DateTime.Now.ToString() + "PushHenKham start.");
                string bvs = config.GetValue<string>("BenhVienChuyenCRM");
                if (!string.IsNullOrEmpty(bvs))
                {   
                    foreach (var mabenhvien in bvs.Split(";"))
                    {
                        try
                        {
                            await henKhamRepo.PushBenhDenKhamToCRM(mabenhvien);
                        }
                        catch (Exception ex)
                        {
                            logger.LogInformation("PushHenKham " + DateTime.Now.ToString() + ": " + mabenhvien + " "+ ex.Message);
                        }
                    }
                }
                else
                {
                    await henKhamRepo.PushBenhDenKhamToCRM("R");
                    await henKhamRepo.PushBenhDenKhamToCRM("E");
                    await henKhamRepo.PushBenhDenKhamToCRM("L");
                    await henKhamRepo.PushBenhDenKhamToCRM("N");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("PushHenKham " + DateTime.Now.ToString() + ": "  + ex.Message);
            }
            
            try
            {
                string bvs = config.GetValue<string>("BenhVienChuyenCRM");
                if (!string.IsNullOrEmpty(bvs))
                {
                    foreach(var mabenhvien in bvs.Split(";"))
                    {
                        try
                        {
                            await henKhamRepo.TuDongUpdateTinhTrangHenKhamToCRM(mabenhvien);
                            await henKhamRepo.TuDongDayHenKhamTheoToaToCRM(mabenhvien);
                        }
                        catch (Exception ex)
                        {
                            logger.LogInformation("TuDongUpdateTinhTrangHenKhamToCRM " + DateTime.Now.ToString() + ": " + mabenhvien + " " + ex.Message);
                        }
                    }
                }
                else
                {
                    logger.LogInformation(DateTime.Now.ToString() + "PushHenKham start.");
                    await henKhamRepo.TuDongUpdateTinhTrangHenKhamToCRM("R");
                    await henKhamRepo.TuDongDayHenKhamTheoToaToCRM("R");
                    await henKhamRepo.TuDongUpdateTinhTrangHenKhamToCRM("E");
                    await henKhamRepo.TuDongDayHenKhamTheoToaToCRM("E");
                    await henKhamRepo.TuDongUpdateTinhTrangHenKhamToCRM("L");
                    await henKhamRepo.TuDongDayHenKhamTheoToaToCRM("L");
                    await henKhamRepo.TuDongUpdateTinhTrangHenKhamToCRM("N");
                    await henKhamRepo.TuDongDayHenKhamTheoToaToCRM("N");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("TuDongUpdateTinhTrangHenKhamToCRM " + DateTime.Now.ToString() + ": " + ex.Message);
            }

            isPushingHenKham =false;
            logger.LogInformation(DateTime.Now.ToString() + "PushHenKham start end");
        }

        private bool GuiMailTuDong = false;   
        private async void SendMailLeads(object state)
        {
            try
            {

                string str = config.GetValue<string>("GuiMailTuDong");
                if (str!=null && str.ToLower().Trim()=="true") 
                {
                    GuiMailTuDong = true;
                }
            }
            catch { 
            }            

            if (GuiMailTuDong)
            {

                logger.LogInformation(DateTime.Now.ToString() + "SendMailLeads 1.");
                LogHelp.Write("SendMailLeads start.");
                // nghỉ tết tây
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && DateTime.Now.Day <= 3 && DateTime.Now.Month == 1)
                {
                    return;
                }
                // nghỉ tất ta
                if (DateTime.Now >= new DateTime(2021, 2, 1) && DateTime.Now <= new DateTime(2021, 2, 7))
                {
                    return;
                }
                if (isRunning)
                {
                    return;
                }
                isRunning = true;

                try
                {
                    logger.LogInformation(DateTime.Now.ToString() + "Send mail weekly start.");
                    LogHelp.Write("Send mail weekly start.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday
                        && DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 16)
                    {
                        await mailRepo.SendMailBaoCaoTuan(reportRepository);
                    }
                    else
                    {
                        LogHelp.Write("Send mail weekly ngoai gio lam viec");
                    }
                    logger.LogInformation(DateTime.Now.ToString() + "Send mail weekly end.");
                    LogHelp.Write("Send mail weekly end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("Send mail weekly error: " + ex.Message);
                }


                try
                {
                    logger.LogInformation(DateTime.Now.ToString() + "Send mail weekly start.");
                    LogHelp.Write("Send mail weekly start.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday
                        && DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 16)
                    {
                        await mailRepo.SendMailBaoCaoFollowupPatientTuanGroup(reportRepository);
                    }
                    else
                    {
                        LogHelp.Write("Send mail weekly ngoai gio lam viec");
                    }
                    logger.LogInformation(DateTime.Now.ToString() + "Send mail weekly end.");
                    LogHelp.Write("Send mail weekly end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("Send mail weekly error: " + ex.Message);
                }


                try
                {
                    LogHelp.Write("Send mail monthly by weekly is starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday
                        && DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 16)
                    {
                        await mailRepo.SendMailBaoCaoThang(reportRepository, false, true);
                    }
                    else
                    {
                        LogHelp.Write("Send mail monthly by weekly ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail monthly end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("SendMailBaoCaoThang: " + ex.Message);
                }

                try
                {
                    logger.LogInformation(DateTime.Now.ToString() + "Send mail monthly is starting.");
                    LogHelp.Write("Send mail monthly is starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday
                        && DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 16)
                    {
                        await mailRepo.SendMailBaoCaoThang(reportRepository);
                    }
                    else
                    {
                        LogHelp.Write("Send mail monthly ngoai gio lam viec");
                    }
                    logger.LogInformation(DateTime.Now.ToString() + "Send mail monthly end.");
                    LogHelp.Write("Send mail monthly end.");
                }
                catch (Exception ex)
                {
                    logger.LogError(DateTime.Now.ToString() + "SendMailBaoCaoThang: " + ex.Message);
                    LogHelp.Write("SendMailBaoCaoThang: " + ex.Message);
                }

                try
                {
                    LogHelp.Write("Send mail bao cao tuan benh vien is starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday
                        && DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 16)
                    {
                        await mailRepo.SendMailBaoCaoFollowupPatientTuanBenhVien(reportRepository);
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao tuan benh vien ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao tuan benh vien end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("bao cao tuan benh vien error: " + ex.Message);
                }

                try
                {
                    LogHelp.Write("Send mail bao cao thang benh vien is starting.");
                    if (//DateTime.Now.DayOfWeek != DayOfWeek.Saturday
                        //&& 
                        DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 16
                        && DateTime.Now.Day >= 1 && DateTime.Now.Day <= 10
                        )
                    {
                        await mailRepo.SendMailBaoCaoFollowupPatientThangBenhVien(reportRepository);
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao thang benh vien ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao thang benh vien end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("bao cao tuan benh vien error: " + ex.Message);
                }

                try
                {
                    LogHelp.Write("Send mail bao cao thang benh vien is starting.");
                    if (//DateTime.Now.DayOfWeek != DayOfWeek.Saturday
                        //&& 
                        DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 16
                        && DateTime.Now.Day >= 1 && DateTime.Now.Day <= 10
                        )
                    {
                        await mailRepo.SendMailBaoCaoFollowupPatientThangGroup(reportRepository);
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao thang benh vien ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao thang benh vien end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("bao cao tuan benh vien error: " + ex.Message);
                }


                try
                {
                    LogHelp.Write("Send mail bao cao CPA and Call tuan benh vien is starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday
                        && DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 16)
                    {
                        await mailRepo.SendMailBaoCaoCPAAndCallBenhVien(reportRepository, "A", true, false);
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao CPA and Call tuan benh vien ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao CPA and Call tuan benh vien end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("bao cao CPA and Call tuan benh vien error: " + ex.Message);
                }

                try
                {
                    LogHelp.Write("Send mail bao cao CPA and Call tháng benh vien is starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday
                        && DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 17
                        && DateTime.Now.Day >= 5 && DateTime.Now.Day <= 10)
                    {
                        await mailRepo.SendMailBaoCaoCPAAndCallBenhVien(reportRepository, "A", false, false);
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao CPA and Call tháng benh vien ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao CPA and Call tháng benh vien end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("bao cao CPA and Call tuan benh vien error: " + ex.Message);
                }


                try
                {
                    LogHelp.Write("Send mail bao cao CPA Tuần is starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday
                        && DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 16)
                    {
                        await mailRepo.SendMailCPAAndCall(reportRepository, false, true, "O");
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao CPA Tuần ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao CPA Tuần end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("baoSend mail bao cao CPA Tuần: " + ex.Message);
                }

                try
                {
                    LogHelp.Write("Send mail bao cao CPA tháng is starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday
                        && DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 16
                        && DateTime.Now.Day >= 5 && DateTime.Now.Day <= 10
                        )
                    {
                        await mailRepo.SendMailCPAAndCall(reportRepository, false, false, "O");
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao CPA tháng ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao CPA Tháng end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("baoSend mail bao cao CPA Tuần: " + ex.Message);
                }

                try
                {
                    LogHelp.Write("Send mail bao cao daily Tuần is starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday
                        && DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 16)
                    {
                        await mailRepo.SendMailDaiLyTuanMat(reportRepository, dailyReportRepository);
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao daily Tuần ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao daily Tuần end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("baoSend mail bao cao CPA Tuần: " + ex.Message);
                }

                try
                {
                    LogHelp.Write("Send mail bao cao daily Tuần is starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday
                        && DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 16)
                    {
                        await mailRepo.SendMailDaiLyTuanDaKhoa(reportRepository, dailyReportRepository);
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao daily Tuần ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao daily Tuần end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("baoSend mail bao cao CPA Tuần: " + ex.Message);
                }

                try
                {
                    LogHelp.Write("Send mail bao cao daily Bệnh viện starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 16)
                    {
                        await mailRepo.SendMailDaiLyBenhVien(reportRepository, dailyReportRepository, "M");
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao daily Bệnh viện ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao daily Bệnh viện end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("baoSend mail daily Bệnh viện: " + ex.Message);
                }
                try
                {
                    LogHelp.Write("Send mail bao cao daily Bệnh viện starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 16)
                    {
                        await mailRepo.SendMailDaiLyBenhVien(reportRepository, dailyReportRepository, "P");
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao daily Bệnh viện ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao daily Bệnh viện end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("baoSend mail daily Bệnh viện: " + ex.Message);
                }
                try
                {
                    LogHelp.Write("Send mail bao cao daily Bệnh viện starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 17)
                    {
                        await mailRepo.SendMailDaiLyBenhVien(reportRepository, dailyReportRepository, "A");
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao daily Bệnh viện ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao daily Bệnh viện end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("baoSend mail daily Bệnh viện: " + ex.Message);
                }
                try
                {
                    LogHelp.Write("Send mail bao cao daily Bệnh viện starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 17)
                    {
                        await mailRepo.SendMailDaiLyBenhVien(reportRepository, dailyReportRepository, "B");
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao daily Bệnh viện ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao daily Bệnh viện end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("baoSend mail daily Bệnh viện: " + ex.Message);
                }
                try
                {
                    LogHelp.Write("Send mail bao cao daily Bệnh viện starting.");
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 17)
                    {
                        await mailRepo.SendMailDaiLyBenhVien(reportRepository, dailyReportRepository, "V");
                    }
                    else
                    {
                        LogHelp.Write("Send mail bao cao daily Bệnh viện ngoai gio lam viec");
                    }
                    LogHelp.Write("Send mail bao cao daily Bệnh viện end.");
                }
                catch (Exception ex)
                {
                    LogHelp.Write("baoSend mail daily Bệnh viện: " + ex.Message);
                }
                try
                {

                    if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday
                        && DateTime.Now.Hour >= 8 && DateTime.Now.Hour <= 16)
                    {
                        await mailRepo.SendMailChiTietLeadsChuaBook(reportRepository);
                    }

                }
                catch (Exception ex)
                {
                    LogHelp.Write("baoSend mail daily Bệnh viện: " + ex.Message);
                }

                isRunning = false;
                logger.LogInformation(DateTime.Now.ToString() + "SendMailLeads end.");
                LogHelp.Write("SendMailLeads end.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // todo:
            _timerLayLeads?.Change(Timeout.Infinite, 0);
            _timerPushCRM?.Change(Timeout.Infinite, 0);
            logger.LogInformation(DateTime.Now.ToString() + "SendMailService StopAsync.");
            LogHelp.Write("SendMailService StopAsync.");

            return Task.CompletedTask;
        }
        public void Dispose()
        {
            logger.LogInformation(DateTime.Now.ToString() + "SendMailService Dispose.");
            LogHelp.Write("SendMailService Dispose.");
            _timerLayLeads?.Dispose();
            _timerPushCRM?.Dispose();
        }
    }
}
