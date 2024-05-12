using System.Text;
using FluentAssertions;
using HebrewAcronymUtil.ResourcesProviders;
using NSubstitute;

namespace HebrewAcronymUtil.Tests.Unit;

public class CategoryAcronymsTests
{
    [Fact]
    public async Task Load_ShouldLoadAcronymsFromAssemblyResources()
    {
        var resourceProvider = Substitute.For<IResourceProvider>();

        const string data = """
                            {
                            "קבה": "קדוש ברוך הוא",
                            "בנא": "בני אדם",
                            "חו": "חס וחלילה"
                            }
                            """;
        var byteArray = Encoding.UTF8.GetBytes(data);
        resourceProvider.GetResourceStream(Arg.Any<string>()).Returns(new MemoryStream(byteArray));

        CategoryAcronyms sut = new(resourceProvider)
        {
            Category = AcronymCategory.Common
        };

        await sut.Load();

        sut.Should().HaveCount(3);
        sut.Should().ContainKey("קבה").WhoseValue.Should().Be("קדוש ברוך הוא");
        sut.Should().ContainKey("בנא").WhoseValue.Should().Be("בני אדם");
        sut.Should().ContainKey("חו").WhoseValue.Should().Be("חס וחלילה");
    }

    [Fact]
    public async Task Load_ShouldCallGetManifestResourceStreamWithResourceNameMatchTheCategory()
    {
        var resourceProvider = Substitute.For<IResourceProvider>();

        const string data = """
                            {"בנא": "בני אדם" }
                            """;

        var byteArray = Encoding.UTF8.GetBytes(data);
        resourceProvider.GetResourceStream(Arg.Any<string>()).Returns(new MemoryStream(byteArray));

        CategoryAcronyms sut = new(resourceProvider)
        {
            Category = AcronymCategory.Common
        };

        var expectedResourceName = Enum.GetName(typeof(AcronymCategory), sut.Category)?.ToLower()!;

        await sut.Load();

        resourceProvider.Received().GetResourceStream(expectedResourceName);
    }
}