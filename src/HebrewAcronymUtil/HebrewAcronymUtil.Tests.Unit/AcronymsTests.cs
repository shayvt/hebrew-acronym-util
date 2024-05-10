using System.Text;
using HebrewAcronymUtil.AssemblyWrapper;
using NSubstitute;

namespace HebrewAcronymUtil.Tests.Unit;

public class AcronymsTests
{
    [Fact]
    public async Task Load()
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
    }
}