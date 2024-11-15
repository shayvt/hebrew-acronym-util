using System;
using System.Collections;
using System.Collections.Generic;

namespace HebrewAcronymUtil;

public abstract class HebrewAcronyms : IEnumerable<KeyValuePair<string, string>>
{
    protected readonly Dictionary<string, string> AcronymsDict = [];

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return AcronymsDict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public string? ConvertAcronymToWords(string acronym)
    {
        if (acronym is null)
        {
            throw new ArgumentNullException(nameof(acronym));
        }

        var cleaned = HebrewAcronymUtils.RemoveAcronymQuoteChars(acronym);

        return ConvertAcronymWithoutQuoteToWords(cleaned);
    }

    public string? ConvertAcronymWithPrefixToWords(string acronym)
    {
        if (acronym is null)
        {
            throw new ArgumentNullException(nameof(acronym));
        }

        var cleaned = HebrewAcronymUtils.RemoveAcronymQuoteChars(acronym);

        var converted = ConvertAcronymWithoutQuoteToWords(cleaned);

        if (converted is not null)
        {
            return converted;
        }

        foreach (var (prefix, acronymWithoutPrefix) in HebrewAcronymUtils.ExtractWordPrefix(cleaned))
        {
            if (prefix is "")
            {
                return "";
            }

            var convertedWithoutPrefix = ConvertAcronymWithoutQuoteToWords(acronymWithoutPrefix);

            if (convertedWithoutPrefix is null)
            {
                continue;
            }

            return $"{prefix}{convertedWithoutPrefix}";
        }

        return "";
    }

    public string? ConvertAcronymWithoutQuoteToWords(string acronym)
    {
        if (acronym is null)
        {
            throw new ArgumentNullException(nameof(acronym));
        }

        return AcronymsDict.GetValueOrDefault(acronym);
    }
}