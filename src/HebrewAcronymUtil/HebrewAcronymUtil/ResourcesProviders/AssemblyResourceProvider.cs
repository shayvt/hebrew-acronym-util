using System;
using System.Collections.Generic;
using System.IO;

namespace HebrewAcronymUtil.ResourcesProviders;

internal static class AssemblyResourceProvider
{
    internal static Stream? GetResourceStream(AcronymCategory category) => _getResourceStream(category);
    
    private static Func<AcronymCategory, Stream?> _getResourceStream = category =>
        typeof(AssemblyResourceProvider).Assembly.GetManifestResourceStream(GetResourceName(category));

    internal static string GetResourceName(AcronymCategory category) =>
        $"{typeof(AssemblyResourceProvider)}.Resources.{Enum.GetName(typeof(AcronymCategory), category)?.ToLower()}.json";

    [ThreadStatic]
    private static Dictionary<AcronymCategory,Stream?>? _testResourcesStreams;
    
    internal static void SetTestGetResourceStream(Dictionary<AcronymCategory,Stream?> resourcesStreams)
    {
        _testResourcesStreams = resourcesStreams;
        _getResourceStream = category => _testResourcesStreams[category];
    }
}