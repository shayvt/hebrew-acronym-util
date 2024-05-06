using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace HebrewAcronymUtil;

internal class AcronymLoader
{
    internal Dictionary<string, string> GetAcronyms(AcronymCategory category)
    {
        var thisAssembly = Assembly.GetExecutingAssembly();

        var resourceName = $"{GetType().Namespace}.Resources.{Enum.GetName(typeof(AcronymCategory), category)}";
        using var stream = thisAssembly.GetManifestResourceStream(resourceName);

        using var reader = new StreamReader(stream);

        reader.ReadToEnd();

        return null;
    }
}