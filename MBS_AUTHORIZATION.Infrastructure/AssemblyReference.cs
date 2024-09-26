using System.Reflection;

namespace MBS_AUTHORIZATION.Infrastructure;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}