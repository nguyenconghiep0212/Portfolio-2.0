using BackEnd.Contexts;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace SignalRWebpack.Hubs;

public class IncidentHub : Hub
{
    private readonly IncidentContext _context;
    private readonly IServiceProvider _serviceProvider;
    private static readonly Random _random = new();
    // Track cancellation tokens per connection
    private static readonly ConcurrentDictionary<string, CancellationTokenSource> _cancellationTokens = new();

    public IncidentHub(IncidentContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    // Start sending random unsolved incidents
    public async Task SendRandomUnsolvedIncidentLoop()
    {
        Debug.WriteLine("Start sending incident");
        var connectionId = Context.ConnectionId;

        // Cancel any existing loop for this connection
        if (_cancellationTokens.TryGetValue(connectionId, out var existingCts))
        {
            existingCts.Cancel();
            existingCts.Dispose();
            _cancellationTokens.TryRemove(connectionId, out _);
        }

        var cts = new CancellationTokenSource();
        _cancellationTokens[connectionId] = cts;
        var token = cts.Token;

        // Capture the Clients reference before the task starts
        var clients = Clients;

        // Start the loop in a background task
        _ = Task.Run(async () =>
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var newIncident = await GenerateAndSendRandomIncident();
                    await clients.All.SendAsync("incidentReceived", newIncident, token);

                    int delay = _random.Next(3, 6) * 1000;
                    await Task.Delay(delay, token);
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("STOP SIGNALR");
            }
            catch (ObjectDisposedException ex)
            {
                Debug.WriteLine($"ObjectDisposedException: {ex.Message}");
            }
            finally
            {
                _cancellationTokens.TryRemove(connectionId, out _);
            }
        }, token);

        // Return immediately so the dispatcher is not blocked
        await Clients.All.SendAsync("onStartScenario", "Scenario Start");
    }

    // Cancel the sending loop for this connection
    public async Task CancelRandomUnsolvedIncidentLoop()
    {
        Debug.WriteLine("CancelRandomUnsolvedIncidentLoop");
        var connectionId = Context.ConnectionId;

        if (_cancellationTokens.TryRemove(connectionId, out var cts))
        {
            Debug.WriteLine("CancelRandomUnsolvedIncidentLoop IF");
            cts.Cancel();
            cts.Dispose();


        }
        await Clients.All.SendAsync("onCancelScenario", "Scenario Cancel");
    }

    // Clean up on disconnect
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        CancelRandomUnsolvedIncidentLoop();
        return base.OnDisconnectedAsync(exception);
    }

    public async Task<BackEnd.Models.Incident> GenerateAndSendRandomIncident()
    {
        // Bounding box
        double minLat = 20.63628976997301;
        double maxLat = 21.008424646499282;
        double minLng = 106.44047531649736;
        double maxLng = 106.76078728140911;

        // Generate random lat/lng
        double lat = minLat + _random.NextDouble() * (maxLat - minLat);
        double lng = minLng + _random.NextDouble() * (maxLng - minLng);

        // Create a new incident
        var incident = new BackEnd.Models.Incident
        {
            Name = "Random Incident",
            City_Code = "27", // Set as needed or random valid code
            Severity = _random.Next(1, 5), // Example: random severity 1-4
            Description = "Auto-generated incident",
            LatLng = $"[{lat},{lng}]",
            Time = DateTime.UtcNow,
            Solved = false,
            Dispatcher_Team_Id = null
        };

        return incident;


    }

}