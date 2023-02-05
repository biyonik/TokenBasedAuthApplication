using System.Reflection;

namespace TokenBasedAuthApplication.DataAccess;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(Assembly).Assembly;
}