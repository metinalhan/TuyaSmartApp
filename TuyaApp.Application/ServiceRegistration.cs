using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TuyaApp.Application.Validations.TuyaAccount;

namespace TuyaApp.Application
{
    // This is a static class that contains an extension method for IServiceCollection.
    public static class ServiceRegistration
    {
        // This is an extension method that adds application services to the specified IServiceCollection.
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // This line of code gets the currently executing assembly.
            var assembly = Assembly.GetExecutingAssembly();

            // This line of code adds AutoMapper to the IServiceCollection using the specified assembly.
            services.AddAutoMapper(assembly);

            //This line of code adds Fluent Validation to the IServiceCollection
            services.AddValidatorsFromAssemblyContaining<CreateNewAccountValidator>();
        }
    }
}
