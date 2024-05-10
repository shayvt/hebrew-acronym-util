using System.Collections.Generic;
using System.Threading.Tasks;
using HebrewAcronymUtil.AssemblyWrapper;

namespace HebrewAcronymUtil;

public class MergedCategoriesAcronyms(in IAssemblyWrapper assemblyWrapper) : Acronyms
{
    private readonly IAssemblyWrapper _assemblyWrapper = assemblyWrapper;

    public required List<AcronymCategory> Categories { get; init; }

    public async Task Load()
    {
        foreach (var category in Categories)
        {
            var categoryAcronyms = new CategoryAcronyms(_assemblyWrapper)
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