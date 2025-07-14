namespace Flyable.StatusCode;

public struct ContentStatusCode
{
    public const int Normal       = 2000;
    public const int Created      = 2001;
    public const int CreateFailed = 2002;
    public const int NotFound     = 2003;
    public const int Updated      = 2004;
    public const int UpdateFailed = 2005;
    public const int Deleted      = 2006;
    public const int DeleteFailed = 2007;
    public const int Empty        = 2008;
    public const int Locked       = 2009;
}