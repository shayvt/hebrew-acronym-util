using System.Text;
using FluentAssertions;
using HebrewAcronymUtil.ResourcesProviders;
using NSubstitute;

namespace HebrewAcronymUtil.Tests.Unit;

public class MergedCategoriesAcronymTests
{
    [Fact]
    public async Task Load_ShouldLoadAcronymsFromAssemblyResources()
    {
        var assemblyWrapper = Substitute.For<IResourceProvider>();

        const string commonData = """
                                  {
                                    "קבה": "קדוש ברוך הוא",
                                    "בנא": "בני אדם",
                                  "חו": "חס וחלילה"  
                                  }
                                  """;
        var commonStream = new MemoryStream(Encoding.UTF8.GetBytes(commonData));
        assemblyWrapper
            .GetResourceStream(Arg.Is<string>(s => s.Contains("common")))
            .Returns(commonStream);

        const string judaismData = """
                                   {
                                   "יצהר": "יצר הרע"  
                                   }
                                   """;
        var judaismStream = new MemoryStream(Encoding.UTF8.GetBytes(judaismData));
        assemblyWrapper.GetResourceStream(Arg.Is<string>(s => s.Contains("judaism")))
            .Returns(judaismStream);

        MergedCategoriesAcronyms sut = new(assemblyWrapper)
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