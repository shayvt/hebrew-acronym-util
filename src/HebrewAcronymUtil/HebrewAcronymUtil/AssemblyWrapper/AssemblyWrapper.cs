using System.IO;
using System.Reflection;

namespace HebrewAcronymUtil.AssemblyWrapper;

internal class AssemblyWrapper(Assembly assembly) : IAssemblyWrapper
{
    public Stream? GetManifestResourceStream(string name) => assembly.GetManifestResourceStream(name);
}