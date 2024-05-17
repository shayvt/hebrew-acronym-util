using System;
using System.IO;

namespace HebrewAcronymUtil.ResourcesProviders;

internal class AssemblyResourceProvider : IResourceProvider
{
    public Stream? GetResourceStream(AcronymCategory category)
    {
        return typeof(AssemblyResourceProvider).Assembly.GetManifestResourceStream(GetResourceName(category));
    }

    internal static string GetResourceName(AcronymCategory category) =>
        $"{typeof(AssemblyResourceProvider)}.Resources.{Enum.GetName(typeof(AcronymCategory), category)?.ToLower()}.json";
}