using ALL.Database;
using Microsoft.EntityFrameworkCore;
using URLshortner.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using System.Text;
using URLshortner.Utils;
using Microsoft.Extensions.Configuration;
using URLshortner.Middlewares;
using URLshortner.Repositories;
using URLshortner.Repositories.Interfaces;
using URLshortner.Models;
using URLshortner.Services;
using URLshortner.Services.Interfaces;
using FluentValidation.AspNetCore;
using URLshortner.Dtos;
using URLshortner.Dtos.Validators;
using FluentValidation;
using URLshortner.Filters;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.CookiePolicy;
using StackExchange.Redis;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using URLshortner.Application;
using URLshortner.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication()
                .AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure middleware pipeline
app.UseRateLimiter();
app.UseExceptionHandler(opts =>
{
    opts.Run(async ctx =>
    {
        var feature = ctx.Features.Get<IExceptionHandlerFeature>();
        if (feature?.Error is not null)
        {
            var handler = ctx.RequestServices.GetRequiredService<GlobalExceptionHandler>();
            await handler.TryHandleAsync(ctx, feature.Error, ctx.RequestAborted);
        }
    });
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();