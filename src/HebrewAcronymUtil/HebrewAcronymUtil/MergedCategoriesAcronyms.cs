using System.Collections.Generic;
using System.Threading.Tasks;
using HebrewAcronymUtil.ResourcesProviders;

namespace HebrewAcronymUtil;

public class MergedCategoriesAcronyms : Acronyms
{
    private readonly IResourceProvider _resourceProvider;
    public required List<AcronymCategory> Categories { get; init; }

    public MergedCategoriesAcronyms()
    {
        _resourceProvider = new AssemblyResourceProvider();
    }

    internal MergedCategoriesAcronyms(IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
    }

    public async Task Load()
    {
        AcronymsDict.Clear();

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