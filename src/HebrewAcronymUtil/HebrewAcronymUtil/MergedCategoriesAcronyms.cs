using System.Collections.Generic;
using System.Threading.Tasks;

namespace HebrewAcronymUtil;

public class MergedCategoriesAcronyms : Acronyms
{
    public required List<AcronymCategory> Categories { get; init; }

    public async Task Load()
    {
        foreach (var category in Categories)
        {
            var categoryAcronyms = new CategoryAcronyms
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