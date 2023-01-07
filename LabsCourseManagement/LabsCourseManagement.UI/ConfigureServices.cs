using LabsCourseManagement.UI.Pages.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace LabsCourseManagement.UI
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services, IWebAssemblyHostEnvironment env)
        {
            services.AddHttpClient<IProfDataService, ProfDataService>
            (
                client => client.BaseAddress
                = new Uri(env.BaseAddress)
            );

            services.AddHttpClient<ICourseDataService, CourseDataService>
                (
                    client => client.BaseAddress
                    = new Uri(env.BaseAddress)
                );

            services.AddHttpClient<IStudentDataService, StudentDataService>
                (
                    client => client.BaseAddress
                    = new Uri(env.BaseAddress)
                );

            services.AddHttpClient<ILaboratoryDataService, LaboratoryDataService>
                (
                    client => client.BaseAddress
                    = new Uri(env.BaseAddress)
                );

            return services;
        }
    }
}