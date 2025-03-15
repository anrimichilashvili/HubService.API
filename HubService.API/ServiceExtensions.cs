
using HubService.Domain.Entities;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;


namespace HubService.API
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddThisLayer(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers()
    .AddOData(static opt =>
    {
        opt.Select().Filter().Expand().OrderBy().SetMaxTop(100).Count()
            .AddRouteComponents("odata", ServiceExtensions.GetEdmModel());
        opt.TimeZone = TimeZoneInfo.Utc;
    }); ;


            //services.AddSwaggerServices();

            return services;
        }
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();


            var tareEntitySet = builder.EntitySet<BetEntity>("Bets");


            builder.EnableLowerCamelCaseForPropertiesAndEnums();
            return builder.GetEdmModel();
        }

    }
}
