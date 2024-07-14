using Flyable.Extensions;
using Flyable.Repositories;
using Flyable.Repositories.DataAccess.DataBaseAccess.Access;
using Flyable.Repositories.DataAccess.DataBaseAccess.IAccess;
using Flyable.Services.IServices;
using Flyable.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


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

//TODO:配置JWT

#region 配置JWT

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();

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

builder.Services.AddDbContext<FlyableUserContext>(optionsBuilder
    =>
{
    // 配置标准日志，执行EFCore时会在控制台打印对应的SQL语句
    var myLoggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole());
    optionsBuilder.UseLoggerFactory(myLoggerFactory);
    optionsBuilder.UseMySql(connStr, new MySqlServerVersion(new Version(8, 0, 33)));
});

#endregion

#region Service依赖注入

builder.Services
    .AddScoped<IUserBaseService, UserBaseService>()
    .AddScoped<IAdminService, AdminService>()
    .AddScoped<IEditorService, EditorService>()
    .AddScoped<INovelService, NovelService>()
    .AddScoped<IPostService, PostService>()
    .AddScoped<ICommentService, CommentService>();

#endregion

#region 数据仓储层依赖注入

builder.Services
    .AddScoped<IUserBaseAccess, UserBaseAccess>()
    .AddScoped<IAdminAccess, AdminAccess>()
    .AddScoped<IEditorAccess, EditorAccess>()
    .AddScoped<INovelAccess, NovelAccess>()
    .AddScoped<IPostAccess, PostAccess>()
    .AddScoped<ICommentAccess, CommentAccess>();

#endregion

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Log.Information("当前环境为开发环境");

    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapGet("/", () => "Hello World! ");
app.MapControllers();
app.Run();