#pragma warning disable CS8981 // 该类型名称仅包含小写 ascii 字符。此类名称可能会成为该语言的保留值。
global using bytes = byte[];
#pragma warning restore CS8981 // 该类型名称仅包含小写 ascii 字符。此类名称可能会成为该语言的保留值。
using System.Globalization;
using Flyable;
using Flyable.Actions;
using Flyable.Extensions;
using Flyable.Services;
using Flyable.ThirdParty;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
#region 配置HttpClient
builder.Services.AddHttpClient();
#endregion
#region 配置ThirdPartyService
builder.Services.AddScoped<IThirdPartyService, WeChatService>();
builder.Services.AddScoped<IThirdPartyService, QQService>();
builder.Services.AddScoped<IThirdPartyService, GitHubService>();
builder.Services.AddScoped<IThirdPartyService, GoogleService>();
builder.Services.AddScoped<IThirdPartyService, MicrosoftService>();
builder.Services.AddScoped<IThirdPartyService, AppleService>();
builder.Services.AddScoped<ThirdPartyAccountService>();
builder.Services.AddScoped<VerifyCodeService>();
#endregion
#region 配置日志

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .WriteTo.File($"{builder.Configuration["LogPath"]}" +
                  $"/Log-In-{DateTime.Today.Year}年{DateTime.Today.Month}月.log")
    .MinimumLevel.Information()
    .CreateLogger();
builder.Logging.AddSerilog();

#endregion

#region 配置Redis缓存

builder.Services.AddDistributedMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = builder.Configuration["Redis:InstanceName"];
});

#endregion

#region 配置Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Flyable",
            Version = builder.Configuration["AppVersion"],
            Description = "Flyable API",
            Contact = new OpenApiContact
            {
                Name = "Flyable 站长",
                Email = "sunhao2024@qq.com",
                Url = new Uri("https://github.com")
            }
        });
});

#endregion

#region 配置跨域策略组

const string allowAllPolicy = "allowAllPolicy";
const string allowMethodsWithPostPutGet = "allowMethodsWithPostPutGet";
builder.Services.AddCors(options => options.AddDefaultPolicy( //默认策略,只允许本地的80和443端口访问,允许任何请求头,允许任何方法
    policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        // policyBuilder.WithOrigins("http://localhost:80/","https://localhost:443/").AllowAnyHeader().AllowAnyMethod();
    }));


builder.Services.AddCors(options => options.AddPolicy(allowAllPolicy, //允许所有请求策略
    policyBuilder => { policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }));

builder.Services.AddCors(options => options.AddPolicy(allowMethodsWithPostPutGet,
    policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:80/", "https://localhost:443/")
            .AllowAnyHeader()
            .WithMethods("POST", "GET", "PUT", "DELETE");
    }));

#endregion

#region 配置JWT

// 临时注释JWT配置以便测试
// builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here"))
        };
    });
builder.Services.AddAuthorization();

#endregion

#region 启动信息

"Flyable后端服务启动中...".LogInfo();

"""
        _____ _             _     _                               _           _
        |  ___| |_   _  __ _| |__ | | ___          ___ _   _ _ __ | |__   __ _| |_
        | |_  | | | | |/ _` | '_ \| |/ _ \        / __| | | | '_ \| '_ \ / _` | __|
        |  _| | | |_| | (_| | |_) | |  __/        \__ \ |_| | | | | | | | (_| | |_
        |_|   |_|\__, |\__,_|_.__/|_|\___|        |___/\__,_|_| |_|_| |_|\__,_|\__|
              |___/
    """.LogInfo();

"""
                          _ooOoo_
                         o8888888o
                         88" . "88
                         (| ^_^ |)
                         O\  =  /O
                      ____/`---'\____
                    .'  \\|     |//  `.
                   /  \\|||  :  |||//  \
                  /  _||||| -:- |||||-  \
                  |   | \\\  -  /// |   |
                  | \_|  ''\---/''  |   |
                  \  .-\__  `-`  ___/-. /
                ___`. .'  /--.--\  `. . ___
              ."" '<  `.___\_<|>_/___.'  >'"".
            | | :  `- \`.;`\ _ /`;.`/ - ` : | |
            \  \ `-.   \_ __\ /__ _/   .-` /  /
      ========`-.____`-.___\_____/___.-`____.-'========
                           `=---='
      ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
         佛祖保佑       永无BUG     永不修改
    """.LogInfo();

#endregion

// Add services to the container.
builder.Services.AddControllers();

//开启内存缓存
builder.Services.AddMemoryCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region 获取数据库连接字符串

var connStr = builder.Configuration["ConnectionStrings:DefaultConnection"];

#endregion


#region 数据库上下文注入

builder.Services.AddDbContext<FlyableUserContext>(optionsBuilder =>
{
    // 配置标准日志，执行EFCore时会在控制台打印对应的SQL语句
    var myLoggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
        .AddConsole()
        .AddSerilog());
    optionsBuilder.UseLoggerFactory(myLoggerFactory);
    optionsBuilder.UseMySql(connStr, new MySqlServerVersion(new Version(8, 0, 33)));
});

#endregion

#region Actions依赖注入
builder.Services.AddScoped<UserAction>();
builder.Services.AddScoped<NovelAction>();
builder.Services.AddScoped<ForumAction>();
#endregion

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
#if DEBUG
if (app.Environment.IsDevelopment())
{
    Log.Information("当前环境为开发环境");
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endif
app.Lifetime.ApplicationStarted.Register(() =>
{
    var currentTimeUtc = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
    var encodedCurrentTimeUtc = System.Text.Encoding.UTF8.GetBytes(currentTimeUtc);
    var options = new DistributedCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromSeconds(20));
    app.Services.GetService<IDistributedCache>()
        ?.Set("cachedTimeUTC", encodedCurrentTimeUtc, options);
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization(); // 启用授权中间件，修复评分接口 500 错误
app.MapGet("/", () => "Hello World! ");
app.MapControllers();
app.Run();