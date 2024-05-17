using System.IO;

namespace HebrewAcronymUtil.ResourcesProviders;

internal interface IResourceProvider
{
    Stream? GetResourceStream(AcronymCategory category);
}