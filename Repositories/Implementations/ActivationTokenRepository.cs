using ALL.Database;
using Microsoft.EntityFrameworkCore;
using URLshortner.Models;
using URLshortner.Repositories.Interfaces;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;

namespace URLshortner.Repositories.Implementations;

public class ActionTokenRepository : IActionTokenRepository
{
    private readonly IDatabase cache;
    private readonly IConfiguration configuration;

    public ActionTokenRepository(IConnectionMultiplexer redis, IConfiguration configuration)
    {
        cache = redis.GetDatabase();
        this.configuration = configuration;
    }

    public async Task AddAsync(string tokenKey, string token)
    {
        await cache.StringSetAsync(tokenKey, token, TimeSpan.FromMinutes(configuration.GetValue<int>("Redis:ActivationTokenLifetimeInMinutes")));
    }

    public async Task<string> GetToken(string tokenKey)
    {
        var token = await cache.StringGetAsync(tokenKey);
        return token;
    }
}
