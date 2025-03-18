using DataAccessLibrary;
using DB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SalesAuto.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SalesAuto.Api.Services
{
    public class GetCRMDataService : IHostedService
    {
        private readonly ILogger<GetCRMDataService> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration config;
        private LeadsRepo leadsRepository;
        SalesAutoDbContext context;
        private IMySqlDataAccess mySqlDataAccess;
        private SqlDataAccess sqlDataAccess;
        private Timer _timerLayLeads;
        public GetCRMDataService(ILogger<GetCRMDataService> logger, IServiceProvider serviceProvider, IConfiguration config)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.config = config;
        }

        [Obsolete]
        public Task StartAsync(CancellationToken cancellationToken)
        {            
            context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<SalesAutoDbContext>();            
            mySqlDataAccess = new MySqlDataAccess(config);
            sqlDataAccess = new SqlDataAccess(config);
            leadsRepository = new LeadsRepo(context, mySqlDataAccess, sqlDataAccess);
            logger.LogInformation("Lay lead is starting.");
            TimeSpan interval = TimeSpan.FromHours(24);
            //calculate time to run the first time & delay to set the timer
            //DateTime.Today gives time of midnight 00.00
            var nextRunTime = DateTime.Today.AddDays(1).AddHours(1);
            var curTime = DateTime.Now;
            var firstInterval = nextRunTime.Subtract(curTime);

            Action action = () =>
            { 
                //var t1 = Task.Delay(firstInterval);
                //t1.Wait();
                //remove inactive accounts at expected time
                 LayDuLieuLeadsAsync(null);
                //now schedule it to be called every 24 hours for future
                // timer repeates call to RemoveScheduledAccounts every 24 hours.
                _timerLayLeads = new Timer(
                    LayDuLieuLeadsAsync,
                    null,
                    TimeSpan.Zero,
                    interval
                );
            };
            // no need to await this call here because this task is scheduled to run much much later.
            Task.Run(action);
            return Task.CompletedTask;
        }

        private bool isrunning = false;

        [Obsolete]
        private async void LayDuLieuLeadsAsync(object state)
        {
            if (isrunning)
            {
                return;
            }
            isrunning = true;

            try
            {
                logger.LogInformation("LoadLichKhamTuCRM is starting.");
                await leadsRepository.LoadLichKhamTuCRMSQL(false);
            }
            catch (Exception ex)
            {
                logger.LogError("LoadLichKhamTuCRM:" + ex.Message);
            }
            try
            {
                logger.LogInformation("LoadLeadsFromGoogle is starting.");
                await leadsRepository.LoadLeadsFromGoogleSQL();              
                
            }
            catch (Exception ex)
            {
                logger.LogError("LoadLeadsFromGoogle:" + ex.Message);
            }
            isrunning = false;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // todo:
            _timerLayLeads?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timerLayLeads?.Dispose();
        }
    }
}
