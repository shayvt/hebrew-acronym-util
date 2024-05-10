using System.IO;

namespace HebrewAcronymUtil.AssemblyWrapper;

public interface IAssemblyWrapper
{
    Stream? GetManifestResourceStream(string name);
}