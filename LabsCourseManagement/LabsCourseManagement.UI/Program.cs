using LabsCourseManagement.UI;
using LabsCourseManagement.UI.Pages.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddHttpClient<IProfDataService, ProfDataService>
    (
        client => client.BaseAddress
        = new Uri(builder.HostEnvironment.BaseAddress)
    );

builder.Services.AddHttpClient<ICourseDataService, CourseDataService>
    (
        client => client.BaseAddress
        = new Uri(builder.HostEnvironment.BaseAddress)
    );

builder.Services.AddHttpClient<IStudentDataService, StudentDataService>
    (
        client => client.BaseAddress
        = new Uri(builder.HostEnvironment.BaseAddress)
    );

builder.Services.AddHttpClient<ILaboratoryDataService, LaboratoryDataService>
    (
        client => client.BaseAddress
        = new Uri(builder.HostEnvironment.BaseAddress)
    );
builder.Services.AddSingleton<IProfDataService, ProfDataService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
await builder.Build().RunAsync();
