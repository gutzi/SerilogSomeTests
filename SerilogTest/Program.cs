using Serilog;
using Serilog.Events;
using SerilogTest;

namespace SerilogTest;
public class Program {

  //----------------------------------------------------------------------------------------------------
  /// <summary>Configuration</summary>
  public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .AddEnvironmentVariables()
    .Build();

  public static void Main(string[] args) {

    Console.Title = "AuthenticationService";
    Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration(Configuration)
      .CreateLogger();
    AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
    try {
      CreateHostBuilder(args).Build().Run();
    }
    catch (Exception ex) {
      Log.Fatal(ex, "Error in Main method.");
      throw;
    }
    finally {
      Log.CloseAndFlush();
    }
  }

  public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
      .UseSerilog()
      .ConfigureWebHostDefaults(webBuilder => {
        webBuilder.UseStartup<Startup>();
      });
}
