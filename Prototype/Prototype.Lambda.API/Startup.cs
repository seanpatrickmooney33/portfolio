using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Prototype.Lambda.API.Service;
using Prototype.Data.Results;
using Prototype.Service;
using Prototype.Service.HttpClient.Mock;
using Prototype.Service.InMemoryDatabase;
using Prototype.Test.Utility;
using EngineServer.Controllers;
using EngineServer.Message;
using EngineServer.Service;
using Moq;

namespace Prototype.Lambda.API
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Add S3 to the ASP.NET Core dependency injection framework.
            services.AddAWSService<Amazon.S3.IAmazonS3>();

            Boolean isDevelopment = Configuration.GetValue<Boolean>("Development", false);

            if (isDevelopment)
            {
                // Set up engine
                IRedisService redisService = new MockRedisService();
                RedisBuffer RedisBuffer = new RedisBuffer();
                Driver driver = new Driver(RedisBuffer, redisService);

                RaceMessageProcessor RMessageProcessor = new RaceMessageProcessor(RedisBuffer);
                VMessageProcessor VMessageProcessor = new VMessageProcessor(RedisBuffer);
                CMessageProcessor CMessageProcessor = new CMessageProcessor(RedisBuffer);
                MessageController MessageController = new MessageController(RMessageProcessor, VMessageProcessor, CMessageProcessor, driver);

                services.AddScoped<IRedisService>(c => redisService);

                #region Populate with Candidate Data
                String fileName = "C20DP";
                String result = FileData.GetFileData(fileName + ".txt");
                //MockMessageClient.SendMessage(fileName + ".txt", "file", result, "key", "send").Wait();
                MessageController.UploadCMessage(result, fileName);

                fileName = "C20PP";
                result = FileData.GetFileData(fileName + ".txt");
                //MockMessageClient.SendMessage(fileName + ".txt", "file", result, "key", "send").Wait();
                MessageController.UploadCMessage(result, fileName);
                #endregion Populate with Candidate Data

                #region Populate with Race Data
                fileName = "R20DP";
                result = FileData.GetFileData(fileName + ".txt");
                //MockMessageClient.SendMessage(fileName + ".txt", "file", result, "key", "send").Wait();
                MessageController.UploadRMessage(result, fileName);

                fileName = "R20PP";
                result = FileData.GetFileData(fileName + ".txt");
                //MockMessageClient.SendMessage(fileName + ".txt", "file", result, "key", "send").Wait();
                MessageController.UploadRMessage(result, fileName);
                #endregion Populate with Race Data

                #region Populate with Vote Result Data
                fileName = "V20DP";
                result = FileData.GetFileData(fileName + ".txt");
                //MockMessageClient.SendMessage(fileName + ".txt", "messageFile", result, "token", "upload").Wait();
                MessageController.UploadVMessage(result, fileName);

                fileName = "V20PP";
                result = FileData.GetFileData(fileName + ".txt");
                //MockMessageClient.SendMessage(fileName + ".txt", "messageFile", result, "token", "upload").Wait();
                MessageController.UploadVMessage(result, fileName);
                #endregion Populate with Vote Result Data


            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Default", "Universal");
            });
        }
    }
}
