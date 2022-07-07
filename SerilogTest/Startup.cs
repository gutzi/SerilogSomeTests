using Microsoft.OpenApi.Models;
using Serilog;

namespace SerilogTest;
public class Startup {
  public Startup(IConfiguration configuration) {
    Configuration = configuration;
  }

  public IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services) {

    services.AddControllers();
    services.AddSwaggerGen();
    services.AddHttpContextAccessor();

  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {


    if (env.IsDevelopment()) {
      app.UseDeveloperExceptionPage();
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging(options => {
      options.EnrichDiagnosticContext = (diagnosticContext, httpContext) => {
        diagnosticContext.Set("HttpRequestClientIP", httpContext.Connection.RemoteIpAddress);
        diagnosticContext.Set("ClientAgent", httpContext.Request.Headers["User-Agent"].ToString() ?? "(unknown)");
        diagnosticContext.Set("UserName", httpContext.User?.Identity?.Name ?? "(anonymous)");
      };
    });

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints => {
      endpoints.MapControllers();
    });
  }
}
