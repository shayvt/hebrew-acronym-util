using System.Text;
using FluentAssertions;
using HebrewAcronymUtil.ResourcesProviders;

namespace HebrewAcronymUtil.Tests.Unit;

public class MergedCategoriesAcronymTests
{
    [Fact]
    public async Task Load_ShouldLoadAcronymsFromAssemblyResources()
    {
        const string commonData = """
                                  {
                                    "קבה": "קדוש ברוך הוא",
                                    "בנא": "בני אדם",
                                    "חו": "חס וחלילה"
                                  }
                                  """;
        var commonStream = new MemoryStream(Encoding.UTF8.GetBytes(commonData));

        const string judaismData = """
                                   {
                                   "יצהר": "יצר הרע"    
                                   }
                                   """;
        var judaismStream = new MemoryStream(Encoding.UTF8.GetBytes(judaismData));

        AssemblyResourceProvider.SetTestGetResourceStream(
            new Dictionary<AcronymCategory, Stream?>
            {
                { AcronymCategory.Common, commonStream },
                { AcronymCategory.Judaism, judaismStream }
            });

        MergedCategoriesAcronyms sut = new()
        {
            Categories =
            [
                AcronymCategory.Common,
                AcronymCategory.Judaism
            ]
        };

        await sut.Load();

        sut.Should().HaveCount(4);
        sut.Should().ContainKey("קבה").WhoseValue.Should().Be("קדוש ברוך הוא");
        sut.Should().ContainKey("בנא").WhoseValue.Should().Be("בני אדם");
        sut.Should().ContainKey("חו").WhoseValue.Should().Be("חס וחלילה");
        sut.Should().ContainKey("יצהר").WhoseValue.Should().Be("יצר הרע");
    }
}