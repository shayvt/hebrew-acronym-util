using System.Text;
using FluentAssertions;
using HebrewAcronymUtil.AssemblyWrapper;
using NSubstitute;

namespace HebrewAcronymUtil.Tests.Unit;

public class CategoryAcronymsTests
{
    [Fact]
    public async Task Load_ShouldLoadAcronymsFromAssemblyResources()
    {
        var assemblyWrapper = Substitute.For<IAssemblyWrapper>();

        const string data = """
                            {
                            "קבה": "קדוש ברוך הוא",
                            "בנא": "בני אדם",
                            "חו": "חס וחלילה"
                            }
                            """;
        var byteArray = Encoding.UTF8.GetBytes(data);
        assemblyWrapper.GetManifestResourceStream(Arg.Any<string>()).Returns(new MemoryStream(byteArray));

        CategoryAcronyms sut = new(assemblyWrapper)
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
        var assemblyWrapper = Substitute.For<IAssemblyWrapper>();

        const string data = """
                            {"בנא": "בני אדם" }
                            """;

        var byteArray = Encoding.UTF8.GetBytes(data);
        assemblyWrapper.GetManifestResourceStream(Arg.Any<string>()).Returns(new MemoryStream(byteArray));

        CategoryAcronyms sut = new(assemblyWrapper)
        {
            Category = AcronymCategory.Common
        };

        var expectedResourceName =
            $"{sut.GetType().Namespace}.Resources.{Enum.GetName(typeof(AcronymCategory), sut.Category)?.ToLower()}.json";

        await sut.Load();

        assemblyWrapper.Received().GetManifestResourceStream(expectedResourceName);
    }
}