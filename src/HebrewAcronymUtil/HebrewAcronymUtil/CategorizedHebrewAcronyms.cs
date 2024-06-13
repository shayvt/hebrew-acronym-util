using System.Collections.Generic;
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
            AcronymsDict.Clear();
            return new Dictionary<string, string>();
        }

        return await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream) ??
               new Dictionary<string, string>();
    }

    public async Task Load()
    {
        AcronymsDict.Clear();

        foreach (var category in Categories)
        {
            var acronyms = await GetAcronyms(category);

            foreach (var (key, value) in acronyms)
            {
                AcronymsDict[key] = value;
            }
        }
    }
}