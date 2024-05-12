using System.Collections.Generic;
using System.Threading.Tasks;
using HebrewAcronymUtil.ResourcesProviders;

namespace HebrewAcronymUtil;

public class MergedCategoriesAcronyms(in IResourceProvider resourceProvider) : Acronyms
{
    private readonly IResourceProvider _resourceProvider = resourceProvider;

    public required List<AcronymCategory> Categories { get; init; }

    public async Task Load()
    {
        foreach (var category in Categories)
        {
            var categoryAcronyms = new CategoryAcronyms(_resourceProvider)
            {
                Category = category
            };

            await categoryAcronyms.Load();

            foreach (var (key, value) in categoryAcronyms)
            {
                AcronymsDict[key] = value;
            }
        }
    }
}