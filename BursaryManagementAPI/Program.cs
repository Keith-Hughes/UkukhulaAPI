/// <summary>
/// The program.
/// </summary>

//"Server=tcp:ukukhulaserver.database.windows.net,1433;Initial Catalog=UkukhulaDB;Persist Security Info=False;User ID=bbdadmin;Password=password@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", //"Server=tcp:ukukhulaserver.database.windows.net,1433;Initial Catalog=UkukhulaDB;Persist Security Info=False;User ID=bbdadmin;Password=password@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
