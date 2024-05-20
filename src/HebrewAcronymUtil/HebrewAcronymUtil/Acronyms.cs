using System;
using System.Collections;
using System.Collections.Generic;

namespace HebrewAcronymUtil;

public abstract class Acronyms : IEnumerable<KeyValuePair<string, string>>
{
    protected Dictionary<string, string> AcronymsDict = new();

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return AcronymsDict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool IsAcronym(string? word)
    {
        if (word is null || word.Length <= 2)
        {
            return false;
        }

        if (word[^1] is '\'')
        {
            var isAcronym = true;

            for (var i = 0; isAcronym && i < word.Length - 1; i++)
            {
                isAcronym &= word[i] is >= 'א' and <= 'ת';
            }

            return isAcronym;
        }

        if (word[^2] == '"')
        {
            var isAcronym = true;

            for (var i = 0; isAcronym && i < word.Length - 2; i++)
            {
                isAcronym &= word[i] is >= 'א' and <= 'ת';
            }

            isAcronym &= word[^1] is >= 'א' and <= 'ת';

            return isAcronym;
        }

        return false;
    }

    public string RemoveAcronymChars(string acronym)
    {
        if (acronym is null)
        {
            throw new ArgumentNullException(nameof(acronym));
        }

        return
            acronym.Replace("\"", "")
                .Replace("'", "");
    }

    public string? ConvertAcronymToWords(string acronym)
    {
        if (acronym is null)
        {
            throw new ArgumentNullException(nameof(acronym));
        }

        var cleaned = RemoveAcronymChars(acronym);

        return AcronymsDict.GetValueOrDefault(cleaned);
    }
}