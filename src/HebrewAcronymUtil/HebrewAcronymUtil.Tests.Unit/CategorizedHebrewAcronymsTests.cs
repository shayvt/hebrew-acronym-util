using System.Text;
using FluentAssertions;
using HebrewAcronymUtil.ResourcesProviders;
using HebrewAcronymUtil.Tests.Unit.TestUtils;
using NSubstitute;

namespace HebrewAcronymUtil.Tests.Unit;

public class CategorizedHebrewAcronymsTests
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

        var resourceProvider = Substitute.For<IResourceProvider>();

        resourceProvider
            .GetResourceStream(Arg.Is<AcronymCategory>(a => a == AcronymCategory.Common))
            .Returns(commonStream);

        resourceProvider
            .GetResourceStream(Arg.Is<AcronymCategory>(a => a == AcronymCategory.Judaism))
            .Returns(judaismStream);

        CategorizedHebrewAcronyms sut = new(resourceProvider)
        {
            Categories =
            [
                AcronymCategory.Common,
                AcronymCategory.Judaism
            ]
        };

        await sut.Initialize();

        sut.Should().HaveCount(4);
        sut.Should().ContainKey("קבה").WhoseValue.Should().Be("קדוש ברוך הוא");
        sut.Should().ContainKey("בנא").WhoseValue.Should().Be("בני אדם");
        sut.Should().ContainKey("חו").WhoseValue.Should().Be("חס וחלילה");
        sut.Should().ContainKey("יצהר").WhoseValue.Should().Be("יצר הרע");
    }

    [Fact]
    public async Task Load_ShouldClearAcronymsWhenResourceStreamIsNull()
    {
        var resourceProvider = Substitute.For<IResourceProvider>();

        resourceProvider
            .GetResourceStream(Arg.Any<AcronymCategory>())
            .Returns(null as Stream);

        CategorizedHebrewAcronyms sut = new(resourceProvider)
        {
            Categories = [AcronymCategory.Common]
        };

        await sut.Initialize();

        sut.Should().HaveCount(0);
    }


    [Fact]
    public async Task ConvertAcronymToWord_ShouldReturnMatchingWords()
    {
        const string data = """
                            {
                            "קבה": "קדוש ברוך הוא",
                            "בנא": "בני אדם",
                            "חו": "חס וחלילה"
                            }
                            """;

        var byteArray = Encoding.UTF8.GetBytes(data);

        var resourceProvider = Substitute.For<IResourceProvider>();
        resourceProvider
            .GetResourceStream(Arg.Any<AcronymCategory>())
            .Returns(new MemoryStream(byteArray));

        CategorizedHebrewAcronyms sut = new(resourceProvider)
        {
            Categories = [AcronymCategory.Common]
        };

        await sut.Initialize();
        var acronym = sut.ConvertAcronymToWords("""בנ"א""");

        acronym.Should().Be("בני אדם");
    }

    [Fact]
    public async Task Load_ShouldLoadAcronymsAndMergeDuplicates()
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
                                   "קבה": "קצין בריאות",
                                   "יצהר": "יצר הרע"
                                   }
                                   """;
        var judaismStream = new MemoryStream(Encoding.UTF8.GetBytes(judaismData));

        var resourceProvider = Substitute.For<IResourceProvider>();

        resourceProvider
            .GetResourceStream(Arg.Is<AcronymCategory>(a => a == AcronymCategory.Common))
            .Returns(commonStream);

        resourceProvider
            .GetResourceStream(Arg.Is<AcronymCategory>(a => a == AcronymCategory.Judaism))
            .Returns(judaismStream);

        CategorizedHebrewAcronyms sut = new(resourceProvider)
        {
            Categories =
            [
                AcronymCategory.Common,
                AcronymCategory.Judaism
            ]
        };

        await sut.Initialize();

        sut.Should().HaveCount(4);
        sut.Should().ContainKey("קבה").WhoseValue.Should().Be("קצין בריאות");
        sut.Should().ContainKey("בנא").WhoseValue.Should().Be("בני אדם");
        sut.Should().ContainKey("חו").WhoseValue.Should().Be("חס וחלילה");
        sut.Should().ContainKey("יצהר").WhoseValue.Should().Be("יצר הרע");
    }

    [Fact]
    public void ConvertAcronymWithPrefixToWords_WithValidAcronymAndPrefix_ReturnsConvertedWords()
    {
        // Arrange
        var acronyms = new HebrewAcronymImplementationTest();
        acronyms.AddAcronym("יצהר", "יצר הרע");
        const string acronym = """היצה"ר""";

        // Act
        var result = acronyms.ConvertAcronymWithPrefixToWords(acronym);

        // Assert
        result.Should().Be("היצר הרע");
    }

    [Fact]
    public void ConvertAcronymWithPrefixToWords_WithValidAcronymWithoutPrefix_ReturnsConvertedWords()
    {
        // Arrange
        var acronyms = new HebrewAcronymImplementationTest();
        acronyms.AddAcronym("יצהר", "יצר הרע");
        const string acronym = """יצה"ר""";

        // Act
        var result = acronyms.ConvertAcronymWithPrefixToWords(acronym);

        // Assert
        result.Should().Be("יצר הרע");
    }

    [Fact]
    public void ConvertAcronymWithPrefixToWords_WithInvalidAcronym_ReturnsNull()
    {
        // Arrange
        var acronyms = new HebrewAcronymImplementationTest();
        acronyms.AddAcronym("יצהר", "יצר הרע");
        const string acronym = """בג"ר""";

        // Act
        var result = acronyms.ConvertAcronymWithPrefixToWords(acronym);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ConvertAcronymWithPrefixToWords_WithNullAcronym_ThrowsArgumentNullException()
    {
        // Arrange
        var acronyms = new HebrewAcronymImplementationTest();
        string acronym = null;

        // Act
        Action act = () => acronyms.ConvertAcronymWithPrefixToWords(acronym);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}