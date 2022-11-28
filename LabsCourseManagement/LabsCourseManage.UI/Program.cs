using LabsCourseManage.UI;
using LabsCourseManage.UI.Pages.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IProfDataService, ProfDataService>
    (
        client => client.BaseAddress
        = new Uri(builder.HostEnvironment.BaseAddress)
    );

builder.Services.AddHttpClient<IStudentDataService, StudentDataService>
    (
        client => client.BaseAddress
        = new Uri(builder.HostEnvironment.BaseAddress)
    );

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
