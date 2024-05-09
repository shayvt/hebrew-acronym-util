namespace HebrewAcronymUtil.Tests.Unit;

public class AcronymsTests
{
    [Fact]
    public async Task Load()
    {
        Acronyms sut = new()
        {
           Category = AcronymCategory.Common 
        };
        
        await sut.Load();
    }
}