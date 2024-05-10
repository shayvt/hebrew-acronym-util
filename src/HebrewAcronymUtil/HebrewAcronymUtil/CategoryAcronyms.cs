using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using HebrewAcronymUtil.AssemblyWrapper;

namespace HebrewAcronymUtil;

internal class CategoryAcronyms(IAssemblyWrapper assemblyWrapper) : Acronyms
{
    private readonly IAssemblyWrapper _assemblyWrapper = assemblyWrapper;
    public required AcronymCategory Category { get; init; }

    internal async Task Load()
    {
        var resourceName =
            $"{GetType().Namespace}.Resources.{Enum.GetName(typeof(AcronymCategory), Category)?.ToLower()}.json";

        await using var stream = _assemblyWrapper.GetManifestResourceStream(resourceName);

        if (stream is null)
        {
            return;
        }

        _acronyms = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream) ??
                    new Dictionary<string, string>();
    }

    public string? ConvertAcronymToWord(string acronym)
    {
        if (acronym is null)
        {
            throw new ArgumentNullException(nameof(acronym));
        }

        return _acronyms.GetValueOrDefault(acronym);
    }
}