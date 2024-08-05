using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sync.Common.Constants;
using Sync.Common.Options;
using Sync.Core;
using Sync.Core.Infrustructure;
using Sync.DAL;
using Sync.DAL.Repositories.Implementations;
using Sync.DAL.Repositories.Interfaces;
using Sync.Services.FieldSatClient;
using Sync.Services.FieldSatClient.Caching;
using Sync.Services.MultipleProviders;
using Sync.Services.MultipleProviders.FiledProviders;
using Sync.Services.MultipleProviders.TimeProviders;

namespace Sync.Registery
{
    public static class ServiceRegistery
    {
        public static void RegisterServices(this IServiceCollection services, HostBuilderContext FunctionContext = null)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient<IFieldRepository, FieldRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IPolygonRepository, PolygonRepository>();
            services.AddTransient<ITimePeriodRepository, TimePeriodRepository>();
            services.AddTransient<IFieldSatClient, FieldSatHttpClient>();
            services.AddTransient<IImageSatClient, ImageSatHttpClient>();
            services.AddTransient<ICacheManager<Guid>, InMemoryCacheField>();
            services.AddTransient<ICacheManager<ImageCache>, InMemoryCacheImage>();
            services.AddTransient<IConstantManager, ConstantManager>();
            services.AddTransient<IQueryStringBuilder, TimePeriodQueryStringBuilder>();
            services.AddTransient<IDataProvider<IEnumerable<Guid>>, CacheFieldProvider>();
            services.AddTransient<IDataProvider<IEnumerable<Guid>>, LocalFieldProvider>();
            services.AddTransient<IDataProvider<IEnumerable<Guid>>, SourceFieldProvider>();
            services.AddTransient<IDataProvider<(DateTime? startDate, DateTime? endDate)>, TimePeriodFromParametersProvider>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ICacheManager<Guid>, InMemoryCacheField>();
            services.AddSingleton<ICacheManager<ImageCache>, InMemoryCacheImage>();
            if (FunctionContext != null)
            {
                services.Configure<HttpEndPoints>(FunctionContext.Configuration.GetSection(nameof(HttpEndPoints)));
                services.Configure<SQLConnectionString>(FunctionContext.Configuration.GetSection(nameof(SQLConnectionString)));
                services.Configure<Period>(FunctionContext.Configuration.GetSection(nameof(Period)));
            }

            services.AddMediatR(option => option.RegisterServicesFromAssembly(typeof(AssemblyHolder).Assembly));
            services.AddDbContext<SyncDbContext>();
            services.AddAutoMapper(typeof(AssemblyHolder).Assembly);
            services.AddHttpClient();
        }
    }
}
