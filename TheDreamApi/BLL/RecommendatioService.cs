using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TheDreamApi.Models;

public class RecommendationService : IHostedService, IDisposable
{
    private List<Project> projects;
    private List<User> users;
    private Dictionary<int, List<int>> recommendations;
    private Timer timer;

    public RecommendationService()
    {
        //this.projects = projects;
        //this.users = users;
        this.recommendations = new Dictionary<int, List<int>>();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Start the timer when the application starts
        // timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        Console.WriteLine("hello Do work");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Stop the timer when the application stops
        timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer?.Dispose();
    }

    public List<int> GetRecommendations(int userId)
    {
        if (this.recommendations.ContainsKey(userId))
        {
            return this.recommendations[userId];
        }
        else
        {
            return new List<int>();
        }
    }
}

