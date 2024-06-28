using FluentAssertions;

namespace HebrewAcronymUtil.Tests.Unit;

public class HebrewAcronymUtilsTests
{
    [Fact]
    public void IsAcronym_ShouldReturnTrueForWordContainApostrophesInSecondLastIndex()
    {
        HebrewAcronymUtils.IsAcronym("""בנ"א""").Should().BeTrue();
    }

    [Fact]
    public void IsAcronym_ShouldReturnTrueForWordContainApostrophesWith2Letters()
    {
        HebrewAcronymUtils.IsAcronym("""כ"א""").Should().BeTrue();
    }

    [Fact]
    public void IsAcronym_ShouldReturnTrueForWordContainApostropheInLastIndex()
    {
        HebrewAcronymUtils.IsAcronym("וכו'").Should().BeTrue();
    }

    [Fact]
    public void IsAcronym_ShouldReturnFalseForWordWithoutApostrophes()
    {
        HebrewAcronymUtils.IsAcronym("בנא").Should().BeFalse();
    }

    [Fact]
    public void IsAcronym_ShouldReturnFalseForEnglishAcronymWithApostrophes()
    {
        HebrewAcronymUtils.IsAcronym("""wh"o""").Should().BeFalse();
    }

    [Fact]
    public void IsAcronym_ShouldReturnFalseForEnglishAcronymWithApostrophe()
    {
        HebrewAcronymUtils.IsAcronym("wh'o").Should().BeFalse();
    }

    [Fact]
    public void IsAcronym_ShouldReturnFalseForEmptyString()
    {
        HebrewAcronymUtils.IsAcronym("").Should().BeFalse();
    }

    [Fact]
    public void IsAcronym_ShouldReturnFalseForSingleLetter()
    {
        HebrewAcronymUtils.IsAcronym("ב").Should().BeFalse();
    }

    [Fact]
    public void IsAcronym_ShouldReturnTrueForTwoLettersWithSingleQuote()
    {
        HebrewAcronymUtils.IsAcronym("א'").Should().BeTrue();
    }
    
    [Fact]
    public void IsAcronym_ShouldReturnFalseForTwoLettersWithDoubleQuote()
    {
        HebrewAcronymUtils.IsAcronym("א\"").Should().BeFalse();
    }

    [Fact]
    public void IsAcronym_ShouldReturnFalseForNullString()
    {
        HebrewAcronymUtils.IsAcronym(null).Should().BeFalse();
    }
    
    [Fact]
    public void RemoveAcronymQuoteChars_ShouldThrowForNullAcronym()
    {
        Action act = () => HebrewAcronymUtils.RemoveAcronymQuoteChars(null!);
        
        act.Should().Throw<ArgumentNullException>().WithParameterName("acronym");
    }
    
    [Fact]
    public void RemoveAcronymQuoteChars_ShouldRemoveDoubleQuote()
    {
        HebrewAcronymUtils.RemoveAcronymQuoteChars("""א"כ""").Should().Be("אכ");
    }
    
    [Fact]
    public void RemoveAcronymQuoteChars_ShouldRemoveSingleQuote()
    {
        HebrewAcronymUtils.RemoveAcronymQuoteChars("עמ'").Should().Be("עמ");
    }
    
    [Fact]
    public void ExtractWordPrefix_WithPrefix_ReturnsPrefixAndWord()
    {
        const string word = "הבית";
        var result = HebrewAcronymUtils.ExtractWordPrefix(word);

        result.prefix.Should().Be("ה");
        result.word.Should().Be("בית");
    }

    [Fact]
    public void ExtractWordPrefix_WithoutPrefix_ReturnsEmptyPrefixAndOriginalWord()
    {
        const string word = "בית";
        var result = HebrewAcronymUtils.ExtractWordPrefix(word);

        result.prefix.Should().Be("");
        result.word.Should().Be("בית");
    }

    [Fact]
    public void ExtractWordPrefix_NullWord_ThrowsArgumentNullException()
    {
        string? word = null;
        Action act = () => HebrewAcronymUtils.ExtractWordPrefix(word);

        act.Should().Throw<ArgumentNullException>();
    }
}