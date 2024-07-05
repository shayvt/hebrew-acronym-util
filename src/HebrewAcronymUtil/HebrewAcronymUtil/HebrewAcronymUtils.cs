using System;

namespace HebrewAcronymUtil;

public static class HebrewAcronymUtils
{
    public static bool IsAcronym(string? word)
    {
        if (word is null)
        {
            return false;
        }

        if (word.Length > 1 && word[^1] is '\'')
        {
            var isAcronym = true;

            for (var i = 0; isAcronym && i < word.Length - 1; i++)
            {
                isAcronym &= word[i] is >= 'א' and <= 'ת';
            }

            return isAcronym;
        }

        if (word.Length > 2 && word[^2] == '"')
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

    public static string RemoveAcronymQuoteChars(string acronym)
    {
        if (acronym is null)
        {
            throw new ArgumentNullException(nameof(acronym));
        }

        return acronym
            .Replace("\"", "")
            .Replace("'", "");
    }

    private static readonly string[] WordPrefixes =
    [
        "ה",
        "ל",
        "ב"
    ];

    public static (string prefix, string word) ExtractWordPrefix(string word)
    {
        if (word is null)
        {
            throw new ArgumentNullException(nameof(word));
        }

        foreach (var prefix in WordPrefixes)
        {
            if (word.StartsWith(prefix))
            {
                return (prefix, word[prefix.Length..]);
            }
        }

        return ("", word);
    }
}