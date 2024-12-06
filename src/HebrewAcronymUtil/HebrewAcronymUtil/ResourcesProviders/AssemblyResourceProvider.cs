using System;
using System.IO;

namespace HebrewAcronymUtil.ResourcesProviders;

internal class AssemblyResourceProvider : IResourceProvider
{
    private static readonly string AssemblyName = typeof(AssemblyResourceProvider).Assembly.GetName().Name;

    public Stream? GetResourceStream(AcronymCategory category)
    {
        return typeof(AssemblyResourceProvider).Assembly.GetManifestResourceStream(GetResourceName(category));
    }

    internal static string GetResourceName(AcronymCategory category) =>
        $"{AssemblyName}.Resources.{Enum.GetName(typeof(AcronymCategory), category)?.ToLower()}.json";
}