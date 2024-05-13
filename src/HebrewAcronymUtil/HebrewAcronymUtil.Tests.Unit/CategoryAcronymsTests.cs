using System.Text;
using FluentAssertions;
using HebrewAcronymUtil.ResourcesProviders;

namespace HebrewAcronymUtil.Tests.Unit;

public class CategoryAcronymsTests
{
    [Fact]
    public async Task Load_ShouldLoadAcronymsFromAssemblyResources()
    {
        const string data = """
                            {
                            "קבה": "קדוש ברוך הוא",
                            "בנא": "בני אדם",
                            "חו": "חס וחלילה"
                            }
                            """;
        var byteArray = Encoding.UTF8.GetBytes(data);
        AssemblyResourceProvider.SetTestGetResourceStream(
            new Dictionary<AcronymCategory, Stream?>
            {
                { AcronymCategory.Common, new MemoryStream(byteArray) }
            });

        CategoryAcronyms sut = new()
        {
            Category = AcronymCategory.Common
        };

        await sut.Load();

        sut.Should().HaveCount(3);
        sut.Should().ContainKey("קבה").WhoseValue.Should().Be("קדוש ברוך הוא");
        sut.Should().ContainKey("בנא").WhoseValue.Should().Be("בני אדם");
        sut.Should().ContainKey("חו").WhoseValue.Should().Be("חס וחלילה");
    }
}