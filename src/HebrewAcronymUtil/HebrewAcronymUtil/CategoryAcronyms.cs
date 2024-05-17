using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using HebrewAcronymUtil.ResourcesProviders;

namespace HebrewAcronymUtil;

internal class CategoryAcronyms : Acronyms
{
    private readonly IResourceProvider _resourceProvider;
    public required AcronymCategory Category { get; init; }

    public CategoryAcronyms()
    {
        _resourceProvider = new AssemblyResourceProvider();
    }

    internal CategoryAcronyms(IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
    }

    internal async Task Load()
    {
        await using var stream =
            _resourceProvider.GetResourceStream(Category);

        if (stream is null)
        {
            AcronymsDict.Clear();
            return;
        }

        AcronymsDict = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream) ??
                       new Dictionary<string, string>();
    }
}