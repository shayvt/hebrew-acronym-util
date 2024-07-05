namespace HebrewAcronymUtil.Tests.Unit.TestUtils;

public class HebrewAcronymImplementationTest : HebrewAcronyms
{
    public void AddAcronym(string acronym, string words)
    {
        AcronymsDict[acronym] = words;
    }
}