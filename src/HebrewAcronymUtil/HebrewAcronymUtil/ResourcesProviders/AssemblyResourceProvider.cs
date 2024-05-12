using System.IO;
using System.Reflection;

namespace HebrewAcronymUtil.ResourcesProviders;

internal class AssemblyResourceProvider(Assembly assembly) : IResourceProvider
{
    public Stream? GetResourceStream(string resourceIdentifier) =>
        assembly.GetManifestResourceStream(GetResourceName(resourceIdentifier));

    internal string GetResourceName(string resourceIdentifier) =>
        $"{GetType().Namespace}.Resources.{resourceIdentifier.ToLower()}.json";
}