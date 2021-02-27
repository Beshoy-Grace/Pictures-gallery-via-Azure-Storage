using System.Linq;
using ImageGalleryAPI.Errors;
using ImageGalleryAPI.Infrastructu;
using ImageGalleryAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;

namespace ImageGalleryAPI.Extentions
{
    public static class ApplicationServicesExtensions
    {

      
       public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
       {
            services.AddSingleton<IAzureBlobConnectionFactory, AzureBlobConnectionFactory>();
 	       services.AddSingleton<IAzureBlobService, AzureBlobService>();

           services.AddScoped(x => new BlobServiceClient(config.GetValue<string>("AzureBlobStorageConnectionStrings")));
          
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext => 
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();
                    
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;
      }

    }
}