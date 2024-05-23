using System;
using System.Collections;
using System.Collections.Generic;

namespace HebrewAcronymUtil;

public abstract class HebrewAcronyms : IEnumerable<KeyValuePair<string, string>>
{
    protected readonly Dictionary<string, string> AcronymsDict = new();

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

        var cleaned = HebrewAcronymUtils.RemoveAcronymChars(acronym);

        return AcronymsDict.GetValueOrDefault(cleaned);
    }
}