using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using HebrewAcronymUtil.ResourcesProviders;

namespace HebrewAcronymUtil;

public class CategorizedHebrewAcronyms : HebrewAcronyms, IHebrewAcronyms
{
    private readonly IResourceProvider _resourceProvider;

    public required List<AcronymCategory> Categories { get; init; }

    public CategorizedHebrewAcronyms()
    {
        _resourceProvider = new AssemblyResourceProvider();
    }

    internal CategorizedHebrewAcronyms(IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
    }

    private async Task<Dictionary<string, string>> GetAcronyms(AcronymCategory category)
    {
        await using var stream =
            _resourceProvider.GetResourceStream(category);

        if (stream is null)
        {
            Trace.Write($"Resource not found for {category}");
            return [];
        }

        return await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream) ?? [];
    }

    public async Task Initialize(params HashSet<string> ignoredAcronyms)
    {
        foreach (var category in Categories)
        {
            var acronyms = await GetAcronyms(category);

            foreach (var (key, value) in acronyms)
            {
                if (!ignoredAcronyms.Contains(key))
                {
                    AcronymsDict[key] = value;
                }
            }
        }
    }
}