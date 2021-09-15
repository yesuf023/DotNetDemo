using DataModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Contracts;
using AutoMapper;
using DataModel.Entity;
using DataModel.DTO;
using API.helper;

namespace DotNetDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            var configuration = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();

            });

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddDbContext<EmployeeDbContext>(opt =>
               opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(DbContext), typeof(EmployeeDbContext));
            services.AddScoped(typeof(IEmployee), typeof(EmployeeRepository));
            services.AddScoped(typeof(IDepartment), typeof(DepartmentRepository));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dot Net Demo", Version = "v1", Contact= new OpenApiContact() {Name="Back end team" }, Description="This is demo api project" });
            });
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetDemo v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
