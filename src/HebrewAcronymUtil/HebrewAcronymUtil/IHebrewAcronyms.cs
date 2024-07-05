using System.Threading.Tasks;

namespace HebrewAcronymUtil;

public interface IHebrewAcronyms
{
    string? ConvertAcronymToWords(string acronym);

    public string? ConvertAcronymWithPrefixToWords(string acronym);

    string? ConvertAcronymWithoutQuoteToWords(string acronym);

    public Task Initialize();
}