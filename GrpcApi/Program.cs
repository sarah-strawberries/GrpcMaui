using GrpcShared;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeFirstGrpc();

// Configure kestrel to run using the right ports and protocols
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });

    options.ListenAnyIP(5000, listenOptions =>
    {
        listenOptions.UseHttps();
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

var app = builder.Build();

//client endpoint
app.MapGrpcService<ScheduleOMaticService>();

//server endpoint
app.MapGet("/", () => "IT WORKS!");

app.Run();

public class ScheduleOMaticService : IScheduleOMaticService
{
    public Task<ModifyScheduleResponse> ModifySchedule(ModifyScheduleRequest request)
    {
        return Task.FromResult(new ModifyScheduleResponse
        {
            IsSuccess = true,
            Message = "Band rescheduled",
            UpdatedBookingId = request.BookingId + "NEW"
        });
    }

    public Task<ScheduleResponse> Schedule(ScheduleRequest request)
    {
        return Task.FromResult(new ScheduleResponse
        {
            BookingId = Guid.NewGuid().ToString(),
            IsSuccess = true,
            Message = "Band scheduled"
        });
    }
}

