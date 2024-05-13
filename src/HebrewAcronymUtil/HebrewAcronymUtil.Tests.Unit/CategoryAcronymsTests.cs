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

    [Fact]
    public async Task Load_ShouldClearAcronymsWhenResourceStreamIsNull()
    {
        AssemblyResourceProvider.SetTestGetResourceStream(
            new Dictionary<AcronymCategory, Stream?>
            {
                { AcronymCategory.Common, null }
            });

        CategoryAcronyms sut = new()
        {
            Category = AcronymCategory.Common
        };

        await sut.Load();

        sut.Should().HaveCount(0);
    }

    [Fact]
    public void IsAcronym_ShouldReturnTrueForWordContainApostrophesInSecondLastIndex()
    {
        CategoryAcronyms sut = new()
        {
            Category = AcronymCategory.Common
        };

        sut.IsAcronym("""בנ"א""").Should().BeTrue();
    }

    [Fact]
    public void IsAcronym_ShouldReturnTrueForWordContainApostrophesWith2Letters()
    {
        CategoryAcronyms sut = new()
        {
            Category = AcronymCategory.Common
        };

        sut.IsAcronym("""כ"א""").Should().BeTrue();
    }

    [Fact]
    public void IsAcronym_ShouldReturnTrueForWordContainApostropheInLastIndex()
    {
        CategoryAcronyms sut = new()
        {
            Category = AcronymCategory.Common
        };

        sut.IsAcronym("וכו'").Should().BeTrue();
    }


    [Fact]
    public void IsAcronym_ShouldReturnFalseForWordWithoutApostrophes()
    {
        CategoryAcronyms sut = new()
        {
            Category = AcronymCategory.Common
        };

        sut.IsAcronym("בנא").Should().BeFalse();
    }

    [Fact]
    public void IsAcronym_ShouldReturnFalseForEnglishAcronymWithApostrophes()
    {
        CategoryAcronyms sut = new()
        {
            Category = AcronymCategory.Common
        };

        sut.IsAcronym("""wh"o""").Should().BeFalse();
    }

    [Fact]
    public void IsAcronym_ShouldReturnFalseForEnglishAcronymWithApostrophe()
    {
        CategoryAcronyms sut = new()
        {
            Category = AcronymCategory.Common
        };

        sut.IsAcronym("wh'o").Should().BeFalse();
    }

    [Fact]
    public void IsAcronym_ShouldReturnFalseForEmptyString()
    {
        CategoryAcronyms sut = new()
        {
            Category = AcronymCategory.Common
        };

        sut.IsAcronym("").Should().BeFalse();
    }

    [Fact]
    public void IsAcronym_ShouldReturnFalseForSingleLetter()
    {
        CategoryAcronyms sut = new()
        {
            Category = AcronymCategory.Common
        };

        sut.IsAcronym("ב").Should().BeFalse();
    }
    
    [Fact]
    public void IsAcronym_ShouldReturnFalseForTwoLetters()
    {
        CategoryAcronyms sut = new()
        {
            Category = AcronymCategory.Common
        };

        sut.IsAcronym("א'").Should().BeFalse();
    }

    [Fact]
    public void IsAcronym_ShouldReturnFalseForNullString()
    {
        CategoryAcronyms sut = new()
        {
            Category = AcronymCategory.Common
        };

        sut.IsAcronym(null).Should().BeFalse();
    }
}