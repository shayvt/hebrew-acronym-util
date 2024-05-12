using System.IO;

namespace HebrewAcronymUtil.ResourcesProviders;

public interface IResourceProvider
{
    Stream? GetResourceStream(string resourceIdentifier);
}