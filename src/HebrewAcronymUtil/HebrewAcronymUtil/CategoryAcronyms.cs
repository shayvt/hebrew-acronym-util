using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using HebrewAcronymUtil.ResourcesProviders;

namespace HebrewAcronymUtil;

internal class CategoryAcronyms(in IResourceProvider resourceProvider) : Acronyms
{
    private readonly IResourceProvider _resourceProvider = resourceProvider;
    public required AcronymCategory Category { get; init; }

    internal async Task Load()
    {
        await using var stream =
            _resourceProvider.GetResourceStream(Enum.GetName(typeof(AcronymCategory), Category)?.ToLower());

        if (stream is null)
        {
            return;
        }

        AcronymsDict = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream) ??
                       new Dictionary<string, string>();
    }

    public string? ConvertAcronymToWord(string acronym)
    {
        if (acronym is null)
        {
            throw new ArgumentNullException(nameof(acronym));
        }

        return AcronymsDict.GetValueOrDefault(acronym);
    }
}