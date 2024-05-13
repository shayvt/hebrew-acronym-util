using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using HebrewAcronymUtil.ResourcesProviders;

namespace HebrewAcronymUtil;

internal class CategoryAcronyms : Acronyms
{
    public required AcronymCategory Category { get; init; }

    internal async Task Load()
    {
        await using var stream =
            AssemblyResourceProvider.GetResourceStream(Category);

        if (stream is null)
        {
            AcronymsDict.Clear();
            return;
        }

        AcronymsDict = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream) ??
                       new Dictionary<string, string>();
    }
}