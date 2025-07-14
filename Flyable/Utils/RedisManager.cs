using Flyable.Wraps;
using Microsoft.Extensions.Caching.Distributed;

namespace Flyable.Utils;

public class RedisManager(
    IDistributedCache cache,
    FlyableUserContext context,
    ILogger<RedisManager> logger,
    IConfiguration configuration,
    string redisKeyPrefix,
    string redisKeyExpire,
    string redisKeyVerifyCode,
    string redisKeyVerifyCodeExpire,
    string redisKeyVerifyCodeCount,
    string redisKeyVerifyCodeCountExpire)
{
    private readonly IDistributedCache _cache = cache;
    private readonly FlyableUserContext _context = context;
    private readonly ILogger<RedisManager> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly string _redisKeyPrefix = redisKeyPrefix;
    private readonly string _redisKeyExpire = redisKeyExpire;
    private readonly string _redisKeyVerifyCode = redisKeyVerifyCode;
    private readonly string _redisKeyVerifyCodeExpire = redisKeyVerifyCodeExpire;
    private readonly string _redisKeyVerifyCodeCount = redisKeyVerifyCodeCount;
    private readonly string _redisKeyVerifyCodeCountExpire = redisKeyVerifyCodeCountExpire;
}